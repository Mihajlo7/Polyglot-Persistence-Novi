using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface ICompanyRepository
    {
        Task InsertSeller(SellerModel seller);
        Task<int> InsertManySeller(IEnumerable<SellerModel> sellers);
        Task InsertCourier(CourierModel courier);
        Task<int> InsertManyCourier(IEnumerable<CourierModel> couriers);
    }
}
