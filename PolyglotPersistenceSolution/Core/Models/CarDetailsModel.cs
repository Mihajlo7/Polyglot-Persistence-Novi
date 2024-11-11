using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CarDetailsModel : ProductDetailsModel
    {
        public int YearManufactured {  get; set; }
        public string Model {  get; set; }
        public string SerialNumber { get; set; }
        public string EngineDisplacement { get; set; }
        public string EnginePower { get; set; }
        public string LongDescription { get; set; }
    }
}
