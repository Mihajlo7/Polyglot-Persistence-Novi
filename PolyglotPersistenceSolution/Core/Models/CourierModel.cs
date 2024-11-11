using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CourierModel : CompanyModel
    {
        public decimal DeliveryPrice { get; set; }
        public ICollection<CompanyModel> Contracts { get; set; }
    }
}
