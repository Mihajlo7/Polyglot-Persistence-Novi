using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public enum OrderStatus
    {
        Unknown = 0,
        Pending = 1,
        Shipped = 2,
        Delivered = 3,
    }
}
