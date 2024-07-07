using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.DAL.DapperDAL
{
    public interface IDapperDataAccess
    {
        Task<IEnumerable<T>> GetData<T, P>(string query, P parameters, string conString = "LocalConn");

        Task InsertData<T>(string query, T parameters, string conString = "LocalConn");

        Task<T> GetSingleData<T, P>(string query, P parameters, string conString = "LocalConn");

        Task ExecuteQuery<T>(string query, T parameters, string conString = "LocalConn");

        Task<int> ExecuteScalarAsync<T>(string query, T parameters, string conString = "LocalConn");
    }
}
