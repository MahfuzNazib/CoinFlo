﻿using CoinFlo.BLL.IRepository;
using CoinFlo.BLL.Models.Users;
using CoinFlo.DAL.DapperDAL;
using System.Net;
using System.Net.Sockets;
using CoinFlo.BLL.Exceptions.Auth;

namespace CoinFlo.BLL.Repository
{
    public class UserRepository : IUsersRepository
    {
        public readonly IDapperDataAccess _dapperDataAccess;

        public UserRepository(IDapperDataAccess dapperDataAccess)
        {
            _dapperDataAccess = dapperDataAccess;
        }

        public async Task UserSignUp(Users user)
        {
            string checkEmailQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            var emailExists = await _dapperDataAccess.ExecuteScalarAsync(checkEmailQuery, new { user.Email });

            if (emailExists > 0)
            {
                throw new AuthCustomExceptions.EmailAlreadyInUseException();
            }

            string machineIPAddress = GetLoggedInMachineIPAddress();
            user.LastDeviceIPAddress = machineIPAddress;

            user.OTPCode = GenerateOTPCode();
            user.OTPExpiredTime = GenerateOTPExpiredTime();

            string spName = "SP_AUTH_USER_REGISTRATION";
            await _dapperDataAccess.InsertData(spName, new
            {
                user.UserSecretKey,
                user.FirstName,
                user.LastName,
                user.ContactNo,
                user.Email,
                user.Password,
                user.CountryCode,
                user.Currency,
                user.ProfilePicture,
                user.GmailTokenID,
                user.UserStatus,
                user.RegisteredAt,
                user.LastLoggedIn,
                user.LastDeviceIPAddress,
                user.IsLoggedInFirstTime,
                user.OTPCode,
                user.OTPExpiredTime
            });
        }

        private string? GetLoggedInMachineIPAddress()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);
            IPAddress ipAddress = ipHostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            return ipAddress?.ToString();
        }

        private string GenerateOTPCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString("D6");
        }

        private DateTime GenerateOTPExpiredTime()
        {
            return DateTime.Now.AddMinutes(5);
        }
    }
}
