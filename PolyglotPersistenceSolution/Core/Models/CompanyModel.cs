using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CompanyModel
    {
        public long Id { get; set; }
        public string DunsNumber { get; set; }
        public string Name { get; set; }
        public string Telephone {  get; set; }
        public string Country { get; set; }
    }
}
