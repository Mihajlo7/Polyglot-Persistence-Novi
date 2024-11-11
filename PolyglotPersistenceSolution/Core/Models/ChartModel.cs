using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ChartModel
    {
        public long Id { get; set; }
        public decimal Total { get; set; }
        public ChartStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<ProductModel> Products { get; set; }
        public ConsumerModel Consumer { get; set; }
    }
}
