using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Models.CommonDTO
{
    public class DataBindModel
    {
        public dynamic data {  get; set; }
        
        public PageSummary pageSummary { get; set; }
    }
}
