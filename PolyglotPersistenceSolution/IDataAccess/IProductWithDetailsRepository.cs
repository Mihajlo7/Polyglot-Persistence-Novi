using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace IDataAccess
{
    public interface IProductWithDetailsRepository
    {
        public Task InsertOne(ProductModel product);
        public Task<int> InsertMany(List<ProductModel> product);
        public Task<int> InsertManyBulk(List<ProductModel> products);
    }
}
