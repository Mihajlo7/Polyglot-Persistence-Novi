using Core.Data.Mongo;
using MongoDB.Driver;

namespace MongoDataAccess
{
    public class ProductDataAccess
    {
        private const string CONNECTION_STRING = "mongodb://127.0.0.1:27017";
        private const string DATABASE_NAME = "test";
        private const string PRODUCTS_WITH_COMPANIES = "product_company";

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client=new MongoClient(CONNECTION_STRING);
            var db = client.GetDatabase(DATABASE_NAME);
            return db.GetCollection<T>(collection);
        }

        public async Task<List<ProductWithCompany>> GetAllProductsWithCompany()
        {
            var productsCollection= ConnectToMongo<ProductWithCompany>(PRODUCTS_WITH_COMPANIES);
            var result = await productsCollection.FindAsync(_=>true);
            return result.ToList();
        }

        public  Task CreateProductWithCompany(ProductWithCompany productWithCompany)
        {
            var productsCollection = ConnectToMongo<ProductWithCompany>(PRODUCTS_WITH_COMPANIES);
            return  productsCollection.InsertOneAsync(productWithCompany);
        }

        public Task UpdateProductWithCompany(ProductWithCompany product)
        {
            var productsCollection = ConnectToMongo<ProductWithCompany>(PRODUCTS_WITH_COMPANIES);
            var filter= Builders<ProductWithCompany>.Filter.Eq("Id",product.Id);
            return productsCollection.ReplaceOneAsync(filter,product,new ReplaceOptions { IsUpsert=true});
        }

        public Task DeleteProductWithCompany(ProductWithCompany productWithCompany)
        {
            var productsCollection = ConnectToMongo<ProductWithCompany>(PRODUCTS_WITH_COMPANIES);
            return productsCollection.DeleteOneAsync(p=>p.Id==productWithCompany.Id);
        }
    }
}
