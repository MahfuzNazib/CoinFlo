using CoinFlo.BLL.Models.Users;
using CoinFlo.DAL.DapperDAL;
using System.Net;
using System.Net.Sockets;
using CoinFlo.BLL.Exceptions.Auth;
using System.Diagnostics;
using CoinFlo.BLL.IRepository.IUsersRepository;
using System.Reflection.Metadata.Ecma335;
using CoinFlo.BLL.Models.Auth;

namespace CoinFlo.BLL.Repository.UserRepository
{
    public class UserRepository : IUsersRepository
    {
        public readonly IDapperDataAccess _dapperDataAccess;

        public UserRepository(IDapperDataAccess dapperDataAccess)
        {
            _dapperDataAccess = dapperDataAccess;
        }

        public async Task UpdateUserProfile(int id, string userSecretKey, Users user)
        {
            string SP_NAME = "SP_UPDATE_USER_PROFILE";
            var parameters = new
            {
                Id = id,
                UserSecretKey = userSecretKey,
                user.FirstName,
                user.LastName,
                user.ContactNo,
                user.CountryCode,
                user.Currency,
                user.ProfilePicture,
            };

            await _dapperDataAccess.ExecuteQuery(SP_NAME, parameters);
        }

    }
}
