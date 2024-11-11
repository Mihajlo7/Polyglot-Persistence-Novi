using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.ExternalData
{
    public class CourierEx
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("duns_number")]
        public string DunsNumber { get; set; }
        public string Telephone {  get; set; }
        public string Country { get; set; }
        [JsonPropertyName("delivery_price")]
        public decimal DeliveryPrice { get; set; }

    }
}
