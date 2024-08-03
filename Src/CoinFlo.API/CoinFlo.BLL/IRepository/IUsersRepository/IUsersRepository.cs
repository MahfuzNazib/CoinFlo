using CoinFlo.BLL.Models.Auth;
using CoinFlo.BLL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.IRepository.IUsersRepository
{
    public interface IUsersRepository
    {
        Task UpdateUserProfile(int id, string userSecretKey, Users user);
    }
}
