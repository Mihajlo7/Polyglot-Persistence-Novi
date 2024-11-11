using Core.ExternalData;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccess
{
    public interface ICompanyRepository
    {
        public Task<List<SellerModel>> GetAllSellersBySelect();

        public Task<List<CourierModel>> GetAllCouriersBySelect();

        public Task<List<CompanyModel>> GetAllCompaniesBySelect();

        public Task<int> InsertManyCouriers(List<CourierEx> couriers);
        public Task<int> InsertManyContracts(List<ContractCourierModel> contractCouriers);
    }
}
