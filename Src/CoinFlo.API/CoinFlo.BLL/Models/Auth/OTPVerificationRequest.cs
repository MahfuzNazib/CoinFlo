using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Models.Auth
{
    public class OTPVerificationRequest
    {
        public int otpCode {  get; set; }
        public string userEmail { get; set; }
    }
}
