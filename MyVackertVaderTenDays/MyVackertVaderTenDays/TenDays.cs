using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVackertVaderTenDays
{
    class TenDays
    {
        public TenDays()
        {
            fromdate = DateTime.Today;
        }
        public DateTime fromdate { get; set; }
        public string date { get; set; }
        public string temperature { get; set; }
        public string mintemperature { get; set; }
        public string wind { get; set; }
        public string windarrowimg { get; set; }
        public string rain { get; set; }

    }
}
