using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccess
{
    public interface IProductRepository
    {
        public Task<int> InsertOne(ProductModel productModel);
        public Task<int> InsertMany(List<ProductModel> products);
        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySelect();
        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySelectOptimised();
        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySelectSubQuery();
        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySubQueryApply();
        public Task<List<ProductModel>> GetProductsWithCompaniesByName(string name);
        public Task<List<ProductModel>> GetProductsWithCompaniesByNameWithLike(string name);
        public Task<ProductModel> GetProductWithCompaniesById(long productId);
        public Task<List<ProductModel>> GetProductsWithCompaniesByProduceCountryAndPrice(string produceCountry,decimal productPrice);
        public Task<List<ProductModel>> GetProductsWithCompaniesByNameAndDistributionCountryAndDistributionPrice(string productName,string distributionCountry,decimal distributionPrice);

        public Task<List<ProductModel>> GetProductsWithDetailsByJoin();
        public Task<List<ProductModel>> GetProductsWithDetailsByJoinOptimised();
        public Task<List<ProductModel>> GetProductsWithDetailsBySubQuery();
        public Task<List<ProductModel>> GetProductsWithDetailsBySubQueryUsingApply();
        public Task<List<ProductModel>> GetProductWithDetailByProductId(long productId);
    }
}
