using Core.Models;
using IDataAccess;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using RelationDataAccess.HelperSqlData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationDataAccess.Implementation
{

    public class SqlProductWithSubCategoryRepository : IProductWithSubCategoryRepository
    {
        private readonly string _connectionString;
        private readonly string _database;

        public SqlProductWithSubCategoryRepository(string database)
        {
            _database = database;
            _connectionString = $"Data Source=.;Initial Catalog={database};Integrated Security=True;TrustServerCertificate=True;";
        }

        public async Task<bool> DeleteAllProducts()
        {
            string query = "DELETE FROM ProductsHeader;";

            using var connection= new SqlConnection( _connectionString );
            using var command = new SqlCommand( query, connection );
            
            connection.Open();
            int res=await command.ExecuteNonQueryAsync();
            return res > 0;
        }

        public async Task<bool> DeleteProductById(long id)
        {
            string query = "DELETE FROM ProductsHeader WHERE productId=@Id;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            int res = await command.ExecuteNonQueryAsync();
            return res > 0;
        }

        public async Task<bool> DeleteProductBySubCategoryId(long subCategoryId)
        {
            string query = "DELETE FROM ProductsHeader WHERE subCategoryId=@SubCategoryId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SubCategoryId", subCategoryId);
            connection.Open();
            int res = await command.ExecuteNonQueryAsync();
            return res > 0;

        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            string query = "  SELECT p.productId ProductId,p.name Name,p.price Price, p.subCategoryId SubCategoryId, s.name SubCategoryName,s.categoryId CategoryId,c.Name CategoryName \r\n  FROM ProductsHeader p\r\n  INNER JOIN SubCategories s ON (p.subCategoryId=s.id)\r\n  INNER JOIN Categories c ON (s.categoryId=c.id);";
            using var connection = new SqlConnection( _connectionString );
            using var command = new SqlCommand(query,connection);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetProductsWithSub();
            return res;
        }

        public async Task<List<ProductModel>> GetAllProductsBadWay()
        {
            string query = "SELECT * \r\nFROM ProductsHeader p\r\nINNER JOIN SubCategories s ON (p.subCategoryId=s.id)\r\nINNER JOIN Categories c ON (s.categoryId=c.id);";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetProductsWithSubBadWay();
            return res;
        }

        public async Task<ProductModel> GetProductById(long id)
        {
            string query = "  SELECT p.productId ProductId,p.name Name,p.price Price, p.subCategoryId SubCategoryId, s.name SubCategoryName,s.categoryId CategoryId,c.name CategoryName \r\n  FROM ProductsHeader p\r\n  INNER JOIN SubCategories s ON (p.subCategoryId=s.id)\r\n  INNER JOIN Categories c ON (s.categoryId=c.id)\r\n  WHERE p.productId=@ProductId;";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductId", id);
            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetProductsWithSub();
            return res.First();
        }

        public async Task<List<ProductModel>> GetProductsBySubCategoryId(long subCategoryId)
        {
            string query = "  SELECT p.productId ProductId,p.name Name,p.price Price, p.subCategoryId SubCategoryId, s.name SubCategoryName,s.categoryId CategoryId,c.name CategoryName \r\n  FROM ProductsHeader p\r\n  INNER JOIN SubCategories s ON (p.subCategoryId=s.id)\r\n  INNER JOIN Categories c ON (s.categoryId=c.id)\r\n  WHERE p.subCategoryId=@subCategoryId;";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@subCategoryId", subCategoryId);
            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetProductsWithSub();
            return res;
        }

        public async Task<List<ProductModel>> GetProductsBySubCategoryName(string subCategoryName)
        {
            string query = "  SELECT p.productId ProductId,p.name Name,p.price Price, p.subCategoryId SubCategoryId, s.name SubCategoryName,s.categoryId CategoryId,c.name CategoryName \r\n  FROM ProductsHeader p\r\n  INNER JOIN SubCategories s ON (p.subCategoryId=s.id)\r\n  INNER JOIN Categories c ON (s.categoryId=c.id)\r\n  WHERE s.name=@SubCategoryName;";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SubCategoryName", subCategoryName);
            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetProductsWithSub();
            return res;
        }

        public async Task<int> InsertMany(List<ProductModel> products)
        {
            int count = 0;
            foreach (ProductModel product in products)
            {
                await InsertOne(product);
                count++;
            }
            return count;
        }

        public async Task InsertManyBulk(List<ProductModel> products)
        {
            using var connestion = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateProductWithSubCategoryBulk", connestion);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Clear();
            command.CreateProductWithCompanyBulk(products);

            connestion.Open();

            await command.ExecuteNonQueryAsync();
            
        }

        public async Task InsertOne(ProductModel product)
        {
            using var connestion = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateProductWithSubCategory",connestion);
            command.CommandType= System.Data.CommandType.StoredProcedure;

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@ProductId",product.Id);
            command.Parameters.AddWithValue("@ProductName", product.Name);
            command.Parameters.AddWithValue("@ProductPrice", product.Price);
            command.Parameters.AddWithValue("@SubCategoryName", product.SubCategory.Name);
            command.Parameters.AddWithValue("@CategoryName",product.SubCategory.Category.Name);

            connestion.Open();

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdatePriceByProductId(long productId, decimal price)
        {
            string query = "UPDATE ProductsHeader SET price=@price WHERE productId=@ProductId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@price",price);
            command.Parameters.AddWithValue("@ProductId",productId);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdatePriceBySubCategoryId(long subCategoryId, decimal price)
        {
            string query = "UPDATE ProductsHeader SET price=@price WHERE subCategoryId=@SubCategoryId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@SubCategoryId", subCategoryId);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdatePriceBySubCategoryName(string subCategoryName, decimal price)
        {
            string query = "UPDATE ProductsHeader SET price=@price WHERE subCategoryId=(SELECT id FROM SubCategories WHERE name=@name)";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@name", subCategoryName);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }
    }
}
