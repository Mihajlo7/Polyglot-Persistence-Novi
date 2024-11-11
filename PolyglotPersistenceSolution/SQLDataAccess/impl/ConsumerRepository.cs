using Core.ExternalData;
using Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccess.impl
{
    public  class ConsumerRepository : IConsumerRepository
    {
        private readonly string connectionString = "Data Source=.;Initial Catalog=small_database;Integrated Security=True;TrustServerCertificate=True;";

        public async Task<List<ConsumerModel>> GetAllBySelect()
        {
            List<ConsumerModel> consumers = new();

            string query = "SELECT * FROM Users u INNER JOIN Consumers c ON (u.Id=c.Id) LEFT JOIN CreditCard cc ON (c.Id=cc.consumerId);";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(query,connection);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var consumerId = reader.GetInt64(0);
                var email= reader.GetString(1);
                var passwordHash= reader.GetString(2);
                var passwordSalt = reader[3] as byte[];
                // 4 skip
                var firstName= reader.GetString(5);
                var lastName= reader.GetString(6);
                var birthday= reader.GetDateTime(7);
                var telephone = reader.GetString(8);

                // find consumer with this id
                var foundConsumer= consumers.Find(c=> c.Id == consumerId);
                if (foundConsumer==null)
                {
                    var newConsumer = new ConsumerModel
                    {
                        Id = consumerId,
                        Email = email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        FirstName = firstName,
                        LastName = lastName,
                        BirthDate = birthday,
                        Telephone = telephone,
                        CreditCards = new List<CreditCardModel>()
                    };

                    consumers.Add(newConsumer);
                    foundConsumer = newConsumer;
                }

                // adding new Credit Card
                if (!reader.IsDBNull(9))
                {
                    var creditCard = new CreditCardModel
                    {
                        Id = reader.GetInt64(9),
                        ConsumerId = reader.GetInt64(10),
                        Number = reader.GetString(11),
                        CardType = reader.GetString(12),
                    };

                    foundConsumer.CreditCards.Add(creditCard);
                }
            }
            return consumers;
        }

        public Task<List<ConsumerModel>> GetAllBySelectWithAtributes()
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertConsumerBulk(List<ConsumerEx> consumers)
        {
            int count = 0;
            using var connection= new SqlConnection(connectionString);
            using var command = new SqlCommand("insertConsumer",connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            connection.Open();
            foreach (var consumer in consumers) 
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@FirstName",consumer.FirstName);
                command.Parameters.AddWithValue("@LastName", consumer.LastName);
                command.Parameters.AddWithValue("@BirthDate", consumer.BirthDay);
                command.Parameters.AddWithValue("@Email", consumer.Email);
                command.Parameters.AddWithValue("@Telephone", consumer.Telephone);
                command.Parameters.AddWithValue("@Password", consumer.Password);
                command.Parameters.AddWithValue("@CreditCardType1", consumer.CreditCardType1);
                command.Parameters.AddWithValue("@CreditCardNumber1", consumer.CreditCardNumber1);
                command.Parameters.AddWithValue("@CreditCardType2", consumer.CreditCardType2 ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CreditCardNumber2", consumer.CreditCardNumber2 ?? (object)DBNull.Value);

                
                //connection.Open();
                await command.ExecuteNonQueryAsync();
                count++;
            }
            return count;
        }
    }
}
