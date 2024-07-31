using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Models.PayType
{
    public class PayTypes
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        
        public string UserKey { get; set; }
        
        public string PayBy { get; set; }

        public int Status { get; set; }
    }
}
