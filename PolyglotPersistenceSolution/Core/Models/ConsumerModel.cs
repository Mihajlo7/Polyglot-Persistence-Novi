using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ConsumerModel : UserModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Telephone { get; set; }
        public List<CreditCardModel>? CreditCards { get; set; }
        public List<OrderModel>? Orders { get; set; }
        public List<ConsumerFriendModel>? Friends { get; set; }
    }
}
