using Core.Data.Mongo;
using Core.ExternalData;
using Core.Models;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccess.impl
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string connectionString = "Data Source=.;Initial Catalog=small_database;Integrated Security=True;TrustServerCertificate=True;";

        public async Task<int> InsertManySellers(List<SellerEx> sellers)
        {
            int count = 0;
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("insertSeller", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            connection.Open();
            foreach (SellerEx seller in sellers)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@DunsNumber", seller.DunsNumber);
                command.Parameters.AddWithValue("@Name",seller.Name);
                command.Parameters.AddWithValue("@Telephone", seller.Telephone);
                command.Parameters.AddWithValue("@Country", seller.Country);
                command.Parameters.AddWithValue("@Address", seller.Address);
                command.Parameters.AddWithValue("@City", seller.City);
                command.Parameters.AddWithValue("@HasShop", seller.HasStore);

                await command.ExecuteNonQueryAsync();
                count++;
            }

            return count;
        }

        public async Task InsertBulkCouriers(List<CourierEx> couriers)
        {
            
            using var connection = new SqlConnection(connectionString);
            using var copy = new SqlBulkCopy(connection);
            var (companyTable,courierTable)= toTableCouriers(couriers);
            copy.DestinationTableName = "Companies";

            copy.ColumnMappings.Add(nameof(CourierEx.Id), "Id");
            copy.ColumnMappings.Add(nameof(CourierEx.DunsNumber), "dunsNumber");
            copy.ColumnMappings.Add(nameof(CourierEx.Name), "name");
            copy.ColumnMappings.Add(nameof(CourierEx.Telephone), "telephone");
            copy.ColumnMappings.Add(nameof(CourierEx.Country), "country");
            connection.Open();
            await copy.WriteToServerAsync(companyTable);

            copy.DestinationTableName = "Couriers";
            copy.ColumnMappings.Clear();

            copy.ColumnMappings.Add(nameof(CourierEx.Id), "id");
            copy.ColumnMappings.Add(nameof(CourierEx.DeliveryPrice), "deliveryPrice");

            await copy.WriteToServerAsync(courierTable);

        }

        private (DataTable,DataTable) toTableCouriers(List<CourierEx> couriers)
        {
            var companyTable= new DataTable();
            companyTable.Columns.Add("Id", typeof(Guid));
            companyTable.Columns.Add("dunsNumber", typeof(string));
            companyTable.Columns.Add("name", typeof(string));
            companyTable.Columns.Add("telephone", typeof(string));
            companyTable.Columns.Add("country", typeof(string));

            var courierTable = new DataTable();
            courierTable.Columns.Add("deliveryPrice",typeof(decimal));
            courierTable.Columns.Add("id", typeof(Guid));

            foreach (var courier in couriers)
            {
                companyTable.Rows.Add(courier.Id,courier.DunsNumber,courier.Name,courier.Telephone,courier.Country);
                courierTable.Rows.Add(courier.DeliveryPrice,courier.Id);
            }

            return (companyTable, courierTable);

        }

        public async Task<List<SellerModel>> GetAllSellersBySelect()
        {
            List<SellerModel> sellers = new();

            string query = "SELECT * FROM Companies c INNER JOIN Sellers s ON (c.Id=s.id);";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read()) 
            {
                SellerModel model = new SellerModel()
                {
                    Id = reader.GetInt64(0),
                    DunsNumber=reader.GetString(1),
                    Name = reader.GetString(2),
                    Telephone = reader.GetString(3),
                    Country = reader.GetString(4),
                    Address = reader.GetString(6),
                    City = reader.GetString(7),
                    HasShop= reader.GetBoolean(8),
                };

                sellers.Add(model);
            }
            return sellers;
        }

        public async Task<int> InsertManyCouriers(List<CourierEx> couriers)
        {
            int count = 0;
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("insertCourier", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            connection.Open();
            foreach (CourierEx courier in couriers)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@DunsNumber", courier.DunsNumber);
                command.Parameters.AddWithValue("@Name", courier.Name);
                command.Parameters.AddWithValue("@Telephone", courier.Telephone);
                command.Parameters.AddWithValue("@Country", courier.Country);
                command.Parameters.AddWithValue("@DeliveryPrice", courier.DeliveryPrice);

                await command.ExecuteNonQueryAsync();
                count++;
            }

            return count;
        }

        public async Task<List<CourierModel>> GetAllCouriersBySelect()
        {
            List<CourierModel> couriers= new();
            string query = "SELECT * FROM Companies c INNER JOIN Couriers cc ON (c.Id=cc.Id);";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read()) 
            {
                CourierModel courier = new CourierModel();
                courier.Id = reader.GetInt64(0);
                courier.Name = reader.GetString(2);
                courier.DunsNumber = reader.GetString(1);
                courier.Telephone = reader.GetString(3);
                courier.Country = reader.GetString(4);
                courier.DeliveryPrice= reader.GetDecimal(6);

                couriers.Add(courier);
            }

            return couriers;
        }

        public async Task<List<CompanyModel>> GetAllCompaniesBySelect()
        {
            List<CompanyModel> companies = new();

            string query = "SELECT * FROM Companies c;";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                CompanyModel company = new CompanyModel();
                company.Id = reader.GetInt64(0);
                company.Name = reader.GetString(2);
                company.DunsNumber = reader.GetString(1);
                company.Telephone = reader.GetString(3);
                company.Country = reader.GetString(4);

                companies.Add(company);
            }

            return companies;
        }

        public async Task<int> InsertManyContracts(List<ContractCourierModel> contractCouriers)
        {
            int count = 0;

            string query = "INSERT INTO CourierContacts (courierId,companyId,serialNumContact,endOfContract) " +
                "VALUES(@courierId,@companyId,@serialNumContact,@endOfContract);";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();
            foreach (var contract in contractCouriers)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@courierId", contract.CourierId);
                command.Parameters.AddWithValue("@companyId", contract.Company.Id);
                command.Parameters.AddWithValue("@serialNumContact", contract.SerialNumContract);
                command.Parameters.AddWithValue("@endOfContract", contract.EndOfContract);

                int rowAffected= await command.ExecuteNonQueryAsync();
                count += rowAffected;
            }
            return count;
        }
    }
}
