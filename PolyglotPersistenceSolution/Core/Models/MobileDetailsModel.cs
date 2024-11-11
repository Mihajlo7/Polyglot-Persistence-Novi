using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class MobileDetailsModel : ProductDetailsModel
    {
        public int YearManufactured { get; set; }
        public string SerialNumber { get; set; }
        public string Weight { get; set; }
        public string Storage {  get; set; }
        public string ScreenDiagonal { get; set; }
        public string OperatingSystem { get; set; }
        public string Color { get; set; }
    }
}
