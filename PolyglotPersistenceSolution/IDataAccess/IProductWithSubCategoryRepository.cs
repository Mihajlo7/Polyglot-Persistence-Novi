using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace IDataAccess
{
    public interface IProductWithSubCategoryRepository
    {
        public Task InsertOne(ProductModel product);
        public Task<int> InsertMany(List<ProductModel> product);
        public Task InsertManyBulk(List<ProductModel> products);

        public Task<List<ProductModel>> GetAllProductsBadWay();
        public Task<List<ProductModel>> GetAllProducts();
        public Task<ProductModel> GetProductById(long id);
        public Task<List<ProductModel>> GetProductsBySubCategoryId(long subCategoryId);
        public Task<List<ProductModel>> GetProductsBySubCategoryName(string subCategoryName);
        public Task UpdatePriceBySubCategoryId (long subCategoryId, decimal price);
        public Task UpdatePriceBySubCategoryName (string subCategoryName, decimal price);
        public Task UpdatePriceByProductId (long productId, decimal price);

        public Task<bool> DeleteAllProducts();
        public Task<bool> DeleteProductById (long id);
        public Task<bool> DeleteProductBySubCategoryId(long subCategoryId);
    }
}
