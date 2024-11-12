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

        public Task<ProductModel> GetProductWithDetailByProductId(long productId)
        {
            string query = ProductWithDetail.GetProductWithDetailById;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@ProductId", productId);

            connection.Open();
            return null;
        }

        public Task<int> InsertMany(List<ProductModel> product)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertManyBulk(List<ProductModel> products)
        {
            throw new NotImplementedException();
        }

        public Task InsertOne(ProductModel product)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLongDescriptionByProductId(long productId)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePriceByYearManifactured(int yearManifactured)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStorageByProductId(long productId)
        {
            throw new NotImplementedException();
        }
    }
}
