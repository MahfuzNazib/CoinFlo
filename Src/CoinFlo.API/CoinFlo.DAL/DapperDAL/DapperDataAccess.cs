using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CoinFlo.DAL.DapperDAL
{
    public class DapperDataAccess : IDapperDataAccess
    {
        public readonly IConfiguration _configuration;

        public DapperDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<T>> GetData<T, P>(string query, P parameters, string conString = "LocalConn")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(conString));
            return await connection.QueryAsync<T>(query, parameters, commandType: CommandType.StoredProcedure);
        }


        public async Task InsertData<T>(string query, T parameters, string conString = "LocalConn")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(conString));
            await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> GetSingleData<T, P>(string query, P parameters, string conString = "LocalConn")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(conString));
            return await connection.QueryFirstOrDefaultAsync<T>(query, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task ExecuteQuery<P>(string query, P parameters, string conString = "LocalConn")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(conString));
            await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ExecuteScalarAsync<T>(string query, T parameters, string conString = "LocalConn")
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString(conString)))
            {
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            }
        }
    }
}
