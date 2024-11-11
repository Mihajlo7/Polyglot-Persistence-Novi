using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Mongo
{
    public class Seller : Company
    {
        public string Address { get; set; }
        public string City { get; set; }
        public bool HasMarket { get; set; }
    }
}
