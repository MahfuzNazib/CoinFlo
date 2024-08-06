using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Models.Auth
{
    public class TokenRequest
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
