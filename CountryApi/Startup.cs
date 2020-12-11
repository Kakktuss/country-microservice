using System;
using System.IO;
using System.Reflection;
using Autofac;
using BuildingBlock.Bus.Stan;
using CountryApplication;
using CountryApplication.EntityFrameworkDataAccess;
using LicenseApi.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Prometheus;
using Prometheus.DotNetRuntime;
using Prometheus.SystemMetrics;
using Prometheus.SystemMetrics.Collectors;
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
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v2", new OpenApiInfo {Title = "Country api", Version = "v1"});

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });

            services.AddEntityFrameworkSqlServer();

            services.AddDbContext<CountryContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            10,
                            TimeSpan.FromSeconds(30),
                            null);
                    });

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
                options.RequireHttpsMetadata = false;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("country:read_all", builder => builder.RequirePermission("country:read_all"));

                options.AddPolicy("country:read", builder => builder.RequirePermission("country:read"));
                
                options.AddPolicy("country:create", builder => builder.RequirePermission("country:create"));

                options.AddPolicy("country:delete", builder => builder.RequirePermission("country:delete"));
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

                options.AddPolicy("preProdPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader();

                    builder.WithOrigins("https://seo-reborn.com")
                        .WithMethods("GET", "DELETE", "POST")
                        .AllowCredentials()
                        .AllowAnyHeader();
                });
            });

            services.AddApiVersioning(options => { options.ReportApiVersions = true; });

            services.AddHealthChecks();

            services.AddSystemMetrics(false);
            services.AddSystemMetricCollector<CpuUsageCollector>();
            services.AddSystemMetricCollector<MemoryCollector>();
            services.AddSystemMetricCollector<NetworkCollector>();
            services.AddSystemMetricCollector<LoadAverageCollector>();

            services.AddControllers();
        }

        public virtual void ConfigureMetrics(IApplicationBuilder app)
        {
            var callsCounter = Metrics.CreateCounter("request_total",
                "Counts the requests to the Country API endpoints", new CounterConfiguration()
                {
                    LabelNames = new[] {"method", "endpoint"}
                });

            app.Use((context, next) =>
            {
                callsCounter.WithLabels(context.Request.Method, context.Request.Path);

                return next();
            });

            var collector = DotNetRuntimeStatsBuilder
                .Customize()
                .WithContentionStats()
                .WithJitStats()
                .WithThreadPoolSchedulingStats()
                .WithThreadPoolStats()
                .WithGcStats()
                .WithExceptionStats()
                .StartCollecting();
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(
                new RepositoryAutofacModule(Configuration.GetConnectionString("DefaultConnectionString")));

            builder.RegisterModule(new ServiceAutofacModule());

            var options = StanOptions.GetDefaultOptions();
            options.NatsURL = Configuration
                .GetSection("Bus")
                .GetSection("Nats")["Url"];

            var appName = Configuration
                .GetSection("Bus")
                .GetSection("Nats")["AppName"];

            appName = $"{appName}_{Guid.NewGuid()}";

            builder.RegisterModule(new StanBusesAutofacModule(Configuration
                    .GetSection("Bus")
                    .GetSection("Nats")["ClusterName"],
                appName,
                options));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var corsPolicy = Configuration.GetSection("Http")["Cors"];

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopify store authentication api");
                });
            }

            // Configure CORS
            app.UseCors(corsPolicy);

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                var result = JsonConvert.SerializeObject(new {error = exception.Message});
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

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