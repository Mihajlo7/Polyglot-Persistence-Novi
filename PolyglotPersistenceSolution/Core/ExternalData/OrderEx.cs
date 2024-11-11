using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.ExternalData
{
    public class OrderEx
    {
        public string Address { get; set; }
        [JsonPropertyName("postal_code")]
        public string? PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
