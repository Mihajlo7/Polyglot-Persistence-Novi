using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Models;
using HybridDataAccess.HelperSqlData;
using IDataAccess;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace HybridDataAccess.Implementation
{
    public class HybridProductWithSubCategory : IProductWithSubCategoryRepository
    {
        private readonly string _database;
        private readonly string _connectionString;

        public HybridProductWithSubCategory(string database)
        {
            _database = database;
            _connectionString = $"Data Source=.;Initial Catalog={_database};Integrated Security=True;TrustServerCertificate=True;";
        }

        public async Task<bool> DeleteAllProducts()
        {
            string query = "UPDATE SubCategories SET products=NULL;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            int res =await command.ExecuteNonQueryAsync();

            return res > 0;
        }

        public Task<bool> DeleteProductById(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteProductBySubCategoryId(long subCategoryId)
        {
            string query = "UPDATE SubCategories SET products=NULL;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            int res = await command.ExecuteNonQueryAsync();

            return res > 0;
        }

        public async Task<List<SubCategoryModel>> GetAllProductsH()
        {
            string query = "SELECT id Id, name Name, products Products FROM SubCategories";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetSubCategoriesH();

            return res;
        }

        public Task<List<ProductModel>> GetAllProductsBadWay()
        {
            throw new NotImplementedException();
        }

        public async Task<List<SubCategoryModel>> GetAllProductsHBadWay()
        {
            string query = "SELECT * FROM SubCategories";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetSubCategoriesHBadWay();

            return res;
        }

        public Task<ProductModel> GetProductById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductModel>> GetProductsBySubCategoryId(long subCategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductModel>> GetProductsBySubCategoryName(string subCategoryName)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertMany(List<ProductModel> products)
        {
            string query = "UPDATE SubCategories SET products=JSON_MODIFY(products,'append $',JSON_QUERY(@Product)) WHERE id=@SubCategoryId";
            int count = 0;
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            foreach (var product in products)
            {
                connection.Open();
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Product", JsonSerializer.Serialize(product));
                command.Parameters.AddWithValue("@SubCategoryId", product.Id);


                await command.ExecuteNonQueryAsync();
                count++;
            }
            return count;
        }

        public Task InsertManyBulk(List<ProductModel> products)
        {
            throw new NotImplementedException();
        }

        public async Task InsertManyBulkOpt(List<SubCategoryModel> subCategories)
        {
            string query = "UPDATE SubCategories SET products=@Products WHERE id=@SubCategoryId";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();
            foreach (var subCategory in subCategories)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Products", JsonSerializer.Serialize(subCategory.Products));
                command.Parameters.AddWithValue("@SubCategoryId", subCategory.Id);

            }
            
        }

        public async Task InsertOne(ProductModel product)
        {
            string query = "UPDATE SubCategories SET products=JSON_MODIFY(products,'append $',JSON_QUERY(@Product)) WHERE id=@SubCategoryId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Product",JsonSerializer.Serialize(product));
            command.Parameters.AddWithValue("@SubCategoryId",product.Id);

            connection.Open();
            await command.ExecuteNonQueryAsync();


        }

        public Task UpdatePriceByProductId(long productId, decimal price)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePriceBySubCategoryId(long subCategoryId, decimal price)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePriceBySubCategoryName(string subCategoryName, decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
