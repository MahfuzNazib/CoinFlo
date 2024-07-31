using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Models.Auth
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string UserSecretKey { get; set; }
        public bool UserStatus { get; set; }
        public int IsLoggedInFirstTime { get; set; }
    }
}
