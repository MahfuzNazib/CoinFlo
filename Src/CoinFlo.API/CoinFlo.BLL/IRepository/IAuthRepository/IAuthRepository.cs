using CoinFlo.BLL.Models.Auth;
using CoinFlo.BLL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.IRepository.IAuthRepository
{
    public interface IAuthRepository
    {
        Task UserSignUp(Users user);

        Task<LoginResponse?> UserLogin(string email, string password);

        Task<(bool otpStatus, string otpMessage)> OTPVerification(int otpCode, string userEmail);

        Task ActiveUserStatus(int userId);

        Task<Users> GetCurrentLoggedinUserData(int id, string userSecretKey);

    }
}
