using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ContractCourierModel
    {
        public CompanyModel Company { get; set; }
        public Guid SerialNumContract { get; set; }
        public DateTime EndOfContract { get; set; }
        public long CourierId { get; set; }
    }
}
