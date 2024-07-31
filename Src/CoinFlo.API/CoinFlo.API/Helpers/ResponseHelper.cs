using CoinFlo.BLL.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Cryptography;
using System.Text;

namespace CoinFlo.API.Helpers
{
    public static class ResponseHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static JsonResult GetActionResponse(bool status, string message, dynamic data = null)
        {
            var response = new
            {
                Status = status,
                Message = message,
                Data = data
            };

            return new JsonResult(response);
        }

        public static void StoreLoggedinUserIdKey(HttpResponse response, LoginResponse loginResponse)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(30),
                Secure = true
            };

            string hashedUserId = Convert.ToBase64String(SHA256Hash(Encoding.UTF8.GetBytes(loginResponse.Id.ToString())));
            string hashedUserSecretKey = Convert.ToBase64String(SHA256Hash(Encoding.UTF8.GetBytes(loginResponse.UserSecretKey)));

            response.Cookies.Append("userId", hashedUserId, cookieOptions);
            response.Cookies.Append("userSecretKey", hashedUserSecretKey, cookieOptions);
        }

        private static byte[] SHA256Hash(byte[] input)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(input);
            }
        }

        public static (int UserId, string UserSecretKey) GetCurrentLoggedInUserIDKey()
        {
            var httpContext = _httpContextAccessor?.HttpContext;

            if (httpContext == null)
            {
                throw new UnauthorizedAccessException("HttpContext is not available");
            }

            var userIdClaim = httpContext.User.FindFirst("Id");
            var userSecretKeyClaim = httpContext.User.FindFirst("UserSecretKey");

            if (userIdClaim == null || userSecretKeyClaim == null)
            {
                throw new UnauthorizedAccessException("User claims are missing or invalid.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user ID claim.");
            }

            string userSecretKey = userSecretKeyClaim.Value;

            return (UserId: userId, UserSecretKey: userSecretKey);
        }
    }
}
