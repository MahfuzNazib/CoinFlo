using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Models.Category
{
    public class Categories
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserKey { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // CASH_IN, CASH_OUT
        public bool Status { get; set; } = true;
    }
}
