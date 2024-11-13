using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using IDataAccess;
using Microsoft.Data.SqlClient;
using RelationDataAccess.HelperSqlData;
using RelationDataAccess.Resources;

namespace RelationDataAccess.Implementation
{
    public class SqlProductWithDetailRepository : IProductWithDetailsRepository
    {
        private readonly string _connectionString;
        private readonly string _database;

        public SqlProductWithDetailRepository(string database)
        {
            _database = database;
            _connectionString = $"Data Source=.;Initial Catalog={database};Integrated Security=True;TrustServerCertificate=True;";
        }

        public Task<List<ProductModel>> GetProductsWithDetailByProductName(string productName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductModel>> GetProductsWithDetailsByJoin()
        {
            string query = ProductWithDetail.GetProductWithDetailsLeftJoin;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.CommandTimeout = 300;

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var products = reader.GetProductWithDetailsBadWay();
            return products;
        }

        public async Task<List<ProductModel>> GetProductsWithDetailsByJoinOptimised()
        {
            string query = ProductWithDetail.GetProductsWithDetailsByJoinOptimised;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.CommandTimeout = 300;

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var products = reader.GetProductsWithDetails();
            return products;
        }

        public async Task<List<ProductModel>> GetProductsWithDetailsBySubQuery()
        {
            string query=ProductWithDetail.GetProductsWithDetailsBySubQuery;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.CommandTimeout = 300;

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var products = reader.GetProductsWithDetails();
            return products;
        }

        public async Task<List<ProductModel>> GetProductsWithDetailsBySubQueryUsingApply()
        {
            string query = ProductWithDetail.GetProductsWithDetailsBySubQueryApply;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.CommandTimeout = 300;

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var products = reader.GetProductsWithDetails();
            return products;
        }

        public async Task<ProductModel> GetProductWithDetailByProductId(long productId)
        {
            string query = ProductWithDetail.GetProductWithDetailById;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@ProductId", productId);

            connection.Open();

            using var reader = await command.ExecuteReaderAsync();
            var result = reader.GetProductsWithDetails();
            return result.First();

        }

        public async Task<int> InsertMany(List<ProductModel> products)
        {
            string query = "CreateProductWithDetail";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            int count = 0;

            connection.Open();

            foreach (var product in products)
            {
                command.CreateProductDetailCommand(product);
                await command.ExecuteNonQueryAsync();
                count++;
            }
            return count;
        }

        public Task<int> InsertManyBulk(List<ProductModel> products)
        {
            throw new NotImplementedException();
        }

        public async Task InsertOne(ProductModel product)
        {
            string query = "CreateProductWithDetail";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.CreateProductDetailCommand(product);
            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateLongDescriptionByProductId(long productId)
        {
            string query = "UPDATE Cars SET longDescription=@LongDescription WHERE productDetailId=(SELECT productDetailId FROM ProductDetails WHERE productId=@ProductId);";
            
            using var connection = new SqlConnection(_connectionString);
            using var command= new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductId",productId);
            command.Parameters.AddWithValue("@LongDescription",Guid.NewGuid().ToString());

            connection.Open();

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdatePriceByYearManifactured(int yearManifactured, decimal price)
        {
            string query = "UPDATE ProductsHeader SET price=@Price " +
                "WHERE productId = IN(SELECT productId FROM ProductDetails WHERE productDetailId=(SELECT productDetailId FROM Cars WHERE yearManifactured=@YearManifactured))";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@YearManufactured",yearManifactured);
            command.Parameters.AddWithValue("@Price", price);
            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateStorageByProductId(long productId, int storage)
        {
            string query = "UPDATE Devices SET storage=@Storage WHERE productDetailId = (SELECT productDetailId FROM ProductDetails WHERE productId=@ProductId)";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Storage",$"{storage} gb");
            command.Parameters.AddWithValue("@ProductId",productId);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }
    }
}
