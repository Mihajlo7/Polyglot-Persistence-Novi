using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.ExternalData
{
    public class SellerEx
    {
        [JsonPropertyName("duns_number")]
        public string DunsNumber { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("telephone")]
        public string Telephone {  get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [JsonPropertyName("has_store")]
        public bool HasStore { get; set; }
    }
}
