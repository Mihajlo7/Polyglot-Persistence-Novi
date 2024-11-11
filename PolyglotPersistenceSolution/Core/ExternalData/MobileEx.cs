using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.ExternalData
{
    public class MobileEx : ProductAndDetailsEx
    {
        [JsonPropertyName("year_manifactured")]
        public int YearManifactured {  get; set; }
        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; }
        public double Weight { get; set; }
        public int Storage {  get; set; }
        [JsonPropertyName("screen_diagonal")]
        public double ScreenDiagonal { get; set; }
        [JsonPropertyName("os")]
        public string OperatingSystem { get; set; }
        public string Color { get; set; }

    }
}
