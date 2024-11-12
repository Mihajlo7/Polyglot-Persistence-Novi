using Core.Models;
using IDataAccess;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using RelationDataAccess.HelperSqlData;
using RelationDataAccess.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationDataAccess.Implementation
{
    public class SqlProductWithCompaniesRepository : IProductWithCompaniesRepository
    {
        private readonly string _connectionString;
        private readonly string _database;

        public SqlProductWithCompaniesRepository(string database)
        {
            _database = database;
            _connectionString = $"Data Source=.;Initial Catalog={_database};Integrated Security=True;TrustServerCertificate=True;";
        }

        public async Task<List<ProductModel>> GetAllProductsWithCompaniesBySelect()
        {
            string query = ProductWithCompanyQueries.GetProductsByJoinPlain;

            using var connection = new SqlConnection( _connectionString );
            using var command = new SqlCommand( query, connection );
            command.CommandTimeout = 300;

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var res = reader.GetProductsWithCompanyBadWay();

            return res;
        }

        public async Task<List<ProductModel>> GetAllProductsWithCompaniesBySelectOptimised()
        {
            string query = ProductWithCompanyQueries.GetProductsByJoinOptimised;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.CommandTimeout = 300;

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var res = reader.GetProductsWithCompany();

            return res;
        }

        public async Task<List<ProductModel>> GetAllProductsWithCompaniesBySelectSubQuery()
        {
            string query = ProductWithCompanyQueries.GetProductsBySubQuery;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.CommandTimeout = 300;

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var res = reader.GetProductsWithCompany();

            return res;
        }

        public async Task<List<ProductModel>> GetAllProductsWithCompaniesBySubQueryApply()
        {
            string query = ProductWithCompanyQueries.GetProductsBySubQueryWithApply;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.CommandTimeout = 300;

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var res = reader.GetProductsWithCompany();

            return res;
        }

        public async Task<List<ProductModel>> GetProductsWithCompaniesByName(string name)
        {
            string query = ProductWithCompanyQueries.GetProductsByName;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", $"{name}%");

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var res = reader.GetProductsWithCompany();

            return res;
        }

        public async Task<List<ProductModel>> GetProductsWithCompaniesByNameAndDistributionCountryAndDistributionPrice(string productName, string distributionCountry, decimal distributionPrice)
        {
            string query = ProductWithCompanyQueries.GetProductsByNameAndDistributionCountryAndPrice;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DistributeCountry", distributionCountry);
            command.Parameters.AddWithValue("@ProductName", $"{productName}%");
            command.Parameters.AddWithValue("@DistributePrice", distributionPrice);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var products = reader.GetProductsWithCompany();
            return products;
        }

        public async Task<List<ProductModel>> GetProductsWithCompaniesByNameWithLike(string name)
        {
            string query = ProductWithCompanyQueries.GetProductsByNameWithLike;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var res = reader.GetProductsWithCompany();

            return res;
        }

        public async Task<List<ProductModel>> GetProductsWithCompaniesByProduceCountryAndPrice(string produceCountry, decimal productPrice)
        {
            string query = ProductWithCompanyQueries.GetProductsByCountryAndPrice;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProduceCountry", produceCountry);
            command.Parameters.AddWithValue("@ProductPrice", productPrice);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var products = reader.GetProductsWithCompany();
            return products;
        }

        public async Task<ProductModel> GetProductWithCompaniesById(long productId)
        {
            string query = ProductWithCompanyQueries.GetProductById;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@ProductId", productId);
            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var products= reader.GetProductsWithCompany();
            return products.First();
        }

        public async Task<int> InsertMany(List<ProductModel> products)
        {
            int count = 0;
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateProductWithCompanies", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();
            foreach(var product in products)
            {
                command.CreateProductWithCompany(product);
                await command.ExecuteNonQueryAsync();
                count++;
            }
            return count;
        }

        public async Task<int> InsertManyBulk(List<ProductModel> products)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateProductWithCompaniesBulk", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            DataTable productDataTable = new DataTable();
            DataTable dbDataTable = new DataTable();

            productDataTable.Columns.Add("ProductId",typeof(long));
            productDataTable.Columns.Add("ProduceId",typeof(long));
            productDataTable.Columns.Add("StoreId",typeof(long));

            dbDataTable.Columns.Add("ProductId", typeof(long));
            dbDataTable.Columns.Add("SellerId", typeof(long));
            dbDataTable.Columns.Add("Price", typeof(decimal));
            foreach (var product in products)
            {
                productDataTable.Rows.Add(product.Id,product.Produced.Id,product.Store.Id);
                foreach(var db in product.Distribute)
                {
                    dbDataTable.Rows.Add(db.Product.Id,db.Distributor.Id,db.DistributionPrice);
                }
            }
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@ProductsWithCompanies",productDataTable);
            command.Parameters.AddWithValue("@DistributeProducts",dbDataTable);

            await command.ExecuteNonQueryAsync();

            return 1;
        }

        public async Task<int> InsertManySellers(List<SellerModel> sellers)
        {
            int count = 0;
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateSeller", connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;
            connection.Open();
            foreach (var seller in sellers)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@CompanyId",seller.Id);
                command.Parameters.AddWithValue("@DunsNumber",seller.DunsNumber);
                command.Parameters.AddWithValue("@Name",seller.Name);
                command.Parameters.AddWithValue("@Telephone",seller.Telephone);
                command.Parameters.AddWithValue("@Country",seller.Country);
                command.Parameters.AddWithValue("@Address",seller.Address);
                command.Parameters.AddWithValue("@City",seller.City);
                command.Parameters.AddWithValue("@HasShop",seller.HasShop);

                await command.ExecuteNonQueryAsync();

                count++;
            }
            return count;
        }

        public async Task InsertOne(ProductModel product)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateProductWithCompanies",connection);
            command.CommandType=System.Data.CommandType.StoredProcedure;

            command.CreateProductWithCompany(product);

            await command.ExecuteNonQueryAsync();

        }

        public Task InsertSeller(SellerModel seller)
        {
            throw new NotImplementedException();
        }
    }
}
