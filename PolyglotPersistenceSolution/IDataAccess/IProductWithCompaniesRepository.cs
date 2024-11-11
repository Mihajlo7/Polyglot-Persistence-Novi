using Core.Models;

namespace IDataAccess
{
    public interface IProductWithCompaniesRepository
    {
        public Task InsertOne(ProductModel product);
        public Task<int> InsertMany (List<ProductModel> product);
        public Task<int> InsertManyBulk (List<ProductModel> products);
        public Task InsertSeller(SellerModel seller);
        public Task<int> InsertManySellers(List<SellerModel> sellers);
        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySelect();
        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySelectOptimised();
        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySelectSubQuery();
        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySubQueryApply();
        public Task<List<ProductModel>> GetProductsWithCompaniesByName(string name);
        public Task<List<ProductModel>> GetProductsWithCompaniesByNameWithLike(string name);
        public Task<ProductModel> GetProductWithCompaniesById(long productId);
        public Task<List<ProductModel>> GetProductsWithCompaniesByProduceCountryAndPrice(string produceCountry, decimal productPrice);
        public Task<List<ProductModel>> GetProductsWithCompaniesByNameAndDistributionCountryAndDistributionPrice(string productName, string distributionCountry, decimal distributionPrice);
    }
}
