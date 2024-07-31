using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Models.CommonDTO
{
    public class PageSummary
    {
        public int currentPage { get; set; }
        
        public int perPage { get; set; }
        
        public int firstPage { get; set; }
        
        public int lastPage { get; set; }
        
        public int totalRecord {  get; set; }
    }
}
