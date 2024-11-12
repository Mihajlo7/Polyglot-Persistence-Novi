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

        public Task<List<ProductModel>> GetProductsWithDetailsByJoin();
        public Task<List<ProductModel>> GetProductsWithDetailsByJoinOptimised();
        public Task<List<ProductModel>> GetProductsWithDetailsBySubQuery();
        public Task<List<ProductModel>> GetProductsWithDetailsBySubQueryUsingApply();
        public Task<ProductModel> GetProductWithDetailByProductId(long productId);
        public Task<List<ProductModel>> GetProductsWithDetailByProductName(string productName);

        public Task UpdatePriceByYearManifactured(int yearManifactured);
        public Task UpdateStorageByProductId(long productId);
        public Task UpdateLongDescriptionByProductId(long productId);
    }
}
