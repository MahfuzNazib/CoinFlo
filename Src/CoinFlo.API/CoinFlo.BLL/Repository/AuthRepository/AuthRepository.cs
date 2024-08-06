using CoinFlo.BLL.Exceptions.Auth;
using CoinFlo.BLL.IRepository.IAuthRepository;
using CoinFlo.BLL.Models.Auth;
using CoinFlo.BLL.Models.Users;
using CoinFlo.DAL.DapperDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Repository.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        public readonly IDapperDataAccess _dapperDataAccess;

        public AuthRepository(IDapperDataAccess dapperDataAccess)
        {
            _dapperDataAccess = dapperDataAccess;
        }

        #region SIGNUP
        public async Task UserSignUp(Users user)
        {
            string checkEmailQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            var emailExists = await _dapperDataAccess.ExecuteScalarAsync(checkEmailQuery, new { user.Email });

            if (emailExists > 0)
            {
                throw new AuthCustomExceptions.EmailAlreadyInUseException();
            }

            string? machineIPAddress = GetLoggedInMachineIPAddress();
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

        #endregion

        #region LOGIN
        public async Task<LoginResponse?> UserLogin(string email, string password)
        {
            string query = "SP_USER_LOGIN";
            LoginResponse loginResponse = await _dapperDataAccess.GetSingleData<LoginResponse, dynamic>(query, new { email, password });

            bool validUser = IsUserValid(loginResponse);

            if (validUser)
            {
                if (loginResponse.IsLoggedInFirstTime == 1)
                {
                    await DisabledLoggedinFirstTimeMode(loginResponse.Id, loginResponse.UserSecretKey);
                }
                await UpdateLastLoggedinInformation(loginResponse.Id, loginResponse.UserSecretKey);
                return loginResponse;
            }
            return null;
        }

        private bool IsUserValid(LoginResponse loginResponse)
        {
            if (loginResponse != null)
            {
                return true;
            }

            return false;
        }


        private async Task DisabledLoggedinFirstTimeMode(int Id, string userSecretKey)
        {
            string SP_NAME = "DISABLE_LOGGEDIN_FIRST_TIME";
            await _dapperDataAccess.ExecuteQuery(SP_NAME, new { Id, userSecretKey });
        }

        private async Task UpdateLastLoggedinInformation(int Id, string userSecretKey)
        {
            DateTime lastLoggedIn = DateTime.Now;
            string? LastDeviceIPAddress = GetLoggedInMachineIPAddress();

            string SP_NAME = "UPDATE_USER_LOGGEDIN_INFO";
            await _dapperDataAccess.ExecuteQuery(SP_NAME, new { Id, userSecretKey, lastLoggedIn, LastDeviceIPAddress });
        }

        #endregion

        public async Task<(bool otpStatus, string otpMessage)> OTPVerification(int otpCode, string userEmail)
        {
            const string query = "SP_OTP_Verification";
            var parameters = new
            {
                Email = userEmail,
                OTPCode = otpCode
            };

            var otpVerificationResponse = await _dapperDataAccess.GetSingleData<OtpVerificationResponse, dynamic>(query, parameters);

            if (otpVerificationResponse == null)
            {
                return (false, "Invalid OTP");
            }

            if (otpVerificationResponse.OTPExpiredTime > DateTime.Now)
            {
                await ActiveUserStatus(otpVerificationResponse.Id);
                return (true, "OTP Verified");
            }

            return (false, "OTP is Expired. Try Again!");
        }

        public async Task ActiveUserStatus(int Id)
        {
            const string query = "SP_ACTIVE_USER_STATUS";
            await _dapperDataAccess.ExecuteQuery(query, new { Id });
        }

        public async Task<Users> GetCurrentLoggedinUserData(int id, string userSecretKey)
        {
            string SP_NAME = "SP_GET_USER_DATA";
            var parameters = new
            {
                Id = id,
                UserSecretKey = userSecretKey
            };

            return await _dapperDataAccess.GetSingleData<Users, dynamic>(SP_NAME, parameters);
        }


        #region REFRESH TOKEN
        public async Task SaveRefreshToken(int userId, string refreshToken)
        {
            string query = "SP_SAVE_REFRESH_TOKEN";
            var refreshTokenParam = new
            {
                UserId = userId,
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.Now.AddDays(5),
                IsRevoked = false
            };

            await _dapperDataAccess.ExecuteQuery(query, refreshTokenParam);
        }


        public async Task<RefreshTokenResponse> GetRefreshToken(int userId, string oldToken)
        {
            string query = "SP_GET_REFRESH_TOKEB_BY_USER_ID";

            var paramter = new
            {
                UserId = userId,
                Token = oldToken,
            };

            return await _dapperDataAccess.GetSingleData<RefreshTokenResponse, dynamic>(query, paramter);
        }


        public async Task UpdateRefreshToken(int userId, string newRefreshToken, string oldRefreshToken)
        {
            string query = "SP_UPDATE_REFRESH_TOKEN";
            var parameter = new
            {
                UserId = userId,
                OldRefreshToken = oldRefreshToken,
                NewRefreshToken = newRefreshToken
            };

            await _dapperDataAccess.ExecuteQuery(query, parameter);
        }
        #endregion
    }
}
