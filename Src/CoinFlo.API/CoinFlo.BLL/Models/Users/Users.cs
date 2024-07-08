using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Models.Users
{
    public class Users
    {
        public int Id { get; set; }
        public Guid UserSecretKey { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string CountryCode { get; set; } = "+88"; // Default Country Code is Bangladesh
        public string Currency { get; set; } = "BDT";
        public string ProfilePicture { get; set; } = string.Empty;
        public string GmailTokenID { get; set; } = string.Empty;
        public int UserStatus { get; set; } = 0; // 1 means Active, 0 means Inactive.
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
        public DateTime LastLoggedIn { get; set; } = DateTime.Now;
        public string LastDeviceIPAddress { get; set; } = string.Empty;
        public int IsLoggedInFirstTime { get; set; } = 1; 
        public string OTPCode {  get; set; } = string.Empty;
        public DateTime OTPExpiredTime { get; set; }
    }
}
