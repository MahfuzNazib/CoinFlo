using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Models.Auth
{
    public class OtpVerificationResponse
    {
        public int Id { get; set; }
        public int OTPCode { get; set; }

        public DateTime OTPExpiredTime { get; set; }
    }
}
