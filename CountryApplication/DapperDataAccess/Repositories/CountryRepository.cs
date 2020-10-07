using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountryApplication.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CountryApplication.DapperDataAccess.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly string _connectionString;

        public CountryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<CountryViewModel>> GetCountriesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<CountryViewModel>(
                    @"SELECT c.[Uuid] as Uuid,
                                c.[Name] as Name,
                                c.[Code] as Code
                           FROM [dbo].[Countries] c");

                return result;
            }
        }

        public async Task<CountryViewModel> FindCountryByUuidAsync(Guid uuid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@Uuid", uuid);
                
                var result = await connection.QueryFirstAsync<CountryViewModel>(
                    @"SELECT c.[Uuid] as Uuid,
                                c.[Name] as Name,
                                c.[Code] as Code
                           FROM [dbo].[Countries] c
                           WHERE c.Uuid = @Uuid",
                    dynamicParameters);

                return result;
            }
        }
    }
}