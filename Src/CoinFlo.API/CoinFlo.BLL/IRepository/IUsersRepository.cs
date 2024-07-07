using CoinFlo.BLL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.IRepository
{
    public interface IUsersRepository
    {
        Task UserSignUp(Users user);
    }
}
