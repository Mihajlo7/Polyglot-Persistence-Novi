using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CreditCardModel
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public string CardType { get; set; }
        public long ConsumerId { get; set; }
    }
}
