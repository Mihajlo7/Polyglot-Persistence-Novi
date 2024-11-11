using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.ExternalData
{
    public class ProductAndDetailsEx
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        [JsonPropertyName("short_description")]
        public string ShortDescription { get; set; }
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
        [JsonPropertyName("sub_category")]
        public string SubCategory { get; set; }
        
    }
}
