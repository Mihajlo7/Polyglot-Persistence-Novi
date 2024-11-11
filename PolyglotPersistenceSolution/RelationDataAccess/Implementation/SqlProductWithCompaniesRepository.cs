using Core.Models;
using IDataAccess;
using Microsoft.Data.SqlClient;
using RelationDataAccess.HelperSqlData;
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

        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySelect()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySelectOptimised()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySelectSubQuery()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductModel>> GetAllProductsWithCompaniesBySubQueryApply()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductModel>> GetProductsWithCompaniesByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductModel>> GetProductsWithCompaniesByNameAndDistributionCountryAndDistributionPrice(string productName, string distributionCountry, decimal distributionPrice)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductModel>> GetProductsWithCompaniesByNameWithLike(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductModel>> GetProductsWithCompaniesByProduceCountryAndPrice(string produceCountry, decimal productPrice)
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> GetProductWithCompaniesById(long productId)
        {
            throw new NotImplementedException();
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
