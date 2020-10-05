using System;
using System.IO;
using System.Reflection;
using System.Text;
using Autofac;
using BuildingBlock.Bus.Nats;
using CountryApplication;
using CountryApplication.EntityFrameworkDataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog;
using STAN.Client;

namespace CountryApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;

            WebHostEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v2", new OpenApiInfo {Title = "Country api", Version = "v1"});

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });
            }
            
            services.AddEntityFrameworkSqlServer();

            services.AddDbContext<CountryContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));

                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetSection("ThirdParty").GetSection("Auth0")["TenantName"];
                options.Audience = Configuration.GetSection("ThirdParty").GetSection("Auth0")["Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration.GetSection("ThirdParty").GetSection("Auth0")["Secret"]))
                };
                options.RequireHttpsMetadata = false;
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Shopify store authentication api", Version = "v1"});

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            
            
            services.AddCors(options =>
            {
                options.AddPolicy("developerPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader();
                });
                
                options.AddPolicy("prodPolicy", builder =>
                {
                    builder.WithOrigins("https://seo-reborn.com")
                        .WithMethods("GET", "DELETE", "POST")
                        .AllowCredentials()
                        .AllowAnyHeader();
                });
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });

            services.AddHealthChecks();

            services.AddControllers();
        }

        public virtual void ConfigureMetrics(IApplicationBuilder app)
        {
            var callsCounter = Metrics.CreateCounter("request_total", "Counts the requests to the Country API endpoints", new CounterConfiguration()
            {
                LabelNames = new[] { "method", "endpoint" }
            });

            app.Use((context, next) =>
            {
                callsCounter.WithLabels(context.Request.Method, context.Request.Path);

                return next();
            });
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryAutofacModule());

            builder.RegisterModule(new ServiceAutofacModule());

            var options = StanOptions.GetDefaultOptions();
            options.NatsURL = Configuration
                .GetSection("Bus")
                .GetSection("Nats")["Url"];

            var appName = Configuration
                .GetSection("Bus")
                .GetSection("Nats")["AppName"];

            appName = $"{appName}_{Guid.NewGuid()}";
            
            builder.RegisterModule(new NatsBusesAutofacModule(Configuration
                    .GetSection("Bus")
                    .GetSection("Nats")["ClusterName"], 
                appName, 
                options));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var corsPolicy = "prodPolicy";

            if (env.IsDevelopment())
            {
                corsPolicy = "developerPolicy";

                app.UseDeveloperExceptionPage();
                
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopify store authentication api");
                });
            }

            // Configure CORS
            app.UseCors(corsPolicy);
            
            // Configure Routing
            app.UseRouting();
            
            // Configure Authentication and Authorization
            app.UseAuthentication();

            app.UseAuthorization();

            // Configure API Versioning
            app.UseApiVersioning();

            // Configure Prometheus Metrics
            app.UseMetricServer();

            app.UseHttpMetrics();
            
            ConfigureMetrics(app);

            // Configure serilog to log requests
            app.UseSerilogRequestLogging();

            // Configure endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
        }
    }
}