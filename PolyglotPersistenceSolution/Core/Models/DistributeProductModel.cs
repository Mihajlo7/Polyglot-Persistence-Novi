using Core.Data.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class DistributeProductModel
    {
        public decimal DistributionPrice { get; set; }
        public SellerModel Distributor { get; set; }
        public ProductModel? Product { get; set; }
    }
}
