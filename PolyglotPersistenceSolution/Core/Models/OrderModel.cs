using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class OrderModel
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public string TypeOrder { get; set; }
        public string Address { get; set; }
        public string? PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public CreditCardModel CreditCard { get; set; }
        public ChartModel Chart { get; set; }
        public ConsumerModel Consumer { get; set; }
    }
}
