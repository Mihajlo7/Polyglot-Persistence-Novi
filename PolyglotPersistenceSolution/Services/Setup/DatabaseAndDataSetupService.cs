using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.ExternalData;
using Core.Models;
using Microsoft.Data.SqlClient;
using Services.JsonWorker;
using Services.Scripts;

namespace Services.Setup
{
    public class DatabaseAndDataSetupService
    {
        private readonly string _databaseName;
        private readonly string _connectionString;

        private const string JSON_PATH = "Data";
        private const int SMALL_SET = 5_000;
        private const int MEDIUM_SET = 50_000;
        private const int LARGE_SET = 500_000;

        private const int PRODUCT_ID = 4_000_000;
        private const int PRODUCT_DETAIL_ID = 5_000_000;

        private readonly JsonWorkerClass _jsonWorker;

        public DatabaseAndDataSetupService(string databaseName)
        {
            _databaseName = databaseName;
            _connectionString = $"Data Source=.;Initial Catalog={_databaseName};Integrated Security=True;TrustServerCertificate=True;";
            _jsonWorker = new JsonWorkerClass(JSON_PATH);
        }
        
        public List<SellerModel> GetAllSellers()
        {
            var sellersRaw = _jsonWorker.ReadObjectsFromFile<SellerEx>("SellersRaw.json");
            var sellers= CompanyGeneratorService.ToSellers(sellersRaw);
            return sellers;
        }
        public List<ConsumerFriendModel> GetConsumerFriends(int a,int b,List<ConsumerModel> consumers)
        {
            var consumers1 = _jsonWorker.ReadObjectsFromFile<ConsumerModel>("ConsumersReady.json");
            var consumersFriendShip = consumers1.GenerateConsumerFriends(a,b);
            return consumersFriendShip;
        }
        public List<ProductModel> GenerateProductsSmall()
        {
            List<ProductModel> carProducts = _jsonWorker.ReadObjectsFromFile<CarEx>("Car1.json")
                .ToCarsFromRaws(PRODUCT_ID, PRODUCT_DETAIL_ID, 2000);
            List<ProductModel> mobileProducts = _jsonWorker.ReadObjectsFromFile<MobileEx>("Mobile1.json")
                .ToMobilesFromRaws(PRODUCT_ID+2000,PRODUCT_DETAIL_ID+2000,3000);

            List<ProductModel> products = new List<ProductModel>();
            products.AddRange(carProducts);
            products.AddRange(mobileProducts);
            _jsonWorker.WriteObjectsToFile(products,"Products_Small.json");
            return products;
        }

        public List<ProductModel> GenerateProductsMedium()
        {
            List<ProductModel> carProducts = _jsonWorker.ReadObjectsFromFile<CarEx>("Car1.json")
                .ToCarsFromRaws(PRODUCT_ID, PRODUCT_DETAIL_ID, 20000);
            List<ProductModel> mobileProducts = _jsonWorker.ReadObjectsFromFile<MobileEx>("Mobile1.json")
                .ToMobilesFromRaws(PRODUCT_ID + 20000, PRODUCT_DETAIL_ID + 20000, 30000);

            List<ProductModel> products = new List<ProductModel>();
            products.AddRange(carProducts);
            products.AddRange(mobileProducts);
            _jsonWorker.WriteObjectsToFile(products, "Products_Medium.json");
            return products;
        }

        public List<ProductModel> GenerateProductsLarge() 
        {
            List<CarEx> carExes= new List<CarEx>();
            List<MobileEx> mobileExes= new List<MobileEx>();
            for(int i = 1; i <= 5; i++)
            {
                carExes.AddRange(_jsonWorker.ReadObjectsFromFile<CarEx>($"Car{i}.json"));
                mobileExes.AddRange(_jsonWorker.ReadObjectsFromFile<MobileEx>($"Mobile{i}.json"));
            }
            List<ProductModel> products = new List<ProductModel>();
            products.AddRange(carExes.ToCarsFromRaws(PRODUCT_ID, PRODUCT_DETAIL_ID, 240_000));
            products.AddRange(mobileExes.ToMobilesFromRaws(PRODUCT_ID + 240_000, PRODUCT_DETAIL_ID + 240_000, 260_000));
            _jsonWorker.WriteObjectsToFile(products, "Products_Large.json");
            return products;
        }
        public void SetupRelationDatabase()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            var createTablesCommand = new SqlCommand(RemoveGoStatements(SetupSQLQueiries.Tables),connection,transaction);
            var createTriggersCommand = new SqlCommand(RemoveGoStatements(SetupSQLQueiries.Triggers), connection, transaction);
            var createConsumersProcedures = new SqlCommand(RemoveGoStatements(SetupSQLQueiries.ConsumersSP),connection,transaction);
            var createCompaniesProcedures = new SqlCommand(RemoveGoStatements(SetupSQLQueiries.CompanySP), connection, transaction);
            var createProductsProcedures = new SqlCommand(RemoveGoStatements(SetupSQLQueiries.ProductsSP), connection,transaction);
            var createOrdersProcedures = new SqlCommand(RemoveGoStatements(SetupSQLQueiries.OrderSP), connection, transaction);

            try
            {
                createTablesCommand.ExecuteNonQuery();
                createTriggersCommand.ExecuteNonQuery();
                createConsumersProcedures.ExecuteNonQuery();
                createCompaniesProcedures.ExecuteNonQuery();
                createProductsProcedures.ExecuteNonQuery();
                createOrdersProcedures.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally 
            {
                connection.Close();
            }
        }
        private string RemoveGoStatements(string sqlScript)
        {
            string pattern = @"^\s*GO\s*$";
            return Regex.Replace(sqlScript, pattern, string.Empty, RegexOptions.Multiline | RegexOptions.IgnoreCase).Trim();
        }
    }
}
