using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.ExternalData
{
    public class CarEx : ProductAndDetailsEx
    {
        public int Year { get; set; }
        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; }
        [JsonPropertyName("capacity_engine")]
        public double EngineDisplacement { get; set; }
        [JsonPropertyName("power_engine")]
        public double EnginePower { get; set; }
        [JsonPropertyName("long_description")]
        public string LongDescription { get; set; }
    }
}
