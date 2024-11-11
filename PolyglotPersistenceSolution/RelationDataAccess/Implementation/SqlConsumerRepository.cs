using Core.Models;
using IDataAccess;
using Microsoft.Data.SqlClient;
using RelationDataAccess.HelperSqlData;
using RelationDataAccess.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationDataAccess.Implementation
{
    public class SqlConsumerRepository : IConsumerRepository
    {
        private readonly string _connectionString;
        private readonly string _database;

        public SqlConsumerRepository(string database)
        {
            _database = database;
            _connectionString = $"Data Source=.;Initial Catalog={database};Integrated Security=True;TrustServerCertificate=True;";
        }

        public Task<bool> DeleteConsumerById(long consumerId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteConsumers()
        {
            string query = "DELETE FROM Users";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteConsumersFriends()
        {
            string query = "DELETE FROM ConsumerFriends;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<ConsumerModel> GetConsumerByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(ConsumerQueries.GetByEmail, connection);
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var consumer = reader.GetConsumersFromReader().First();

            return consumer;
        }

        public async Task<ConsumerModel> GetConsumerById(long id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(ConsumerQueries.GetById, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var consumer=reader.GetConsumersFromReader().First();

            return consumer;
        }

        public async Task<List<ConsumerModel>> GetConsumers()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(ConsumerQueries.JoinPlain, connection);
            command.CommandTimeout = 120;

            connection.Open();
            using var reader= await command.ExecuteReaderAsync();

            var consumers=reader.GetConsumersFromReaderBadWay();

            return consumers;
        }

        public async Task<List<ConsumerModel>> GetConsumersByEmailAndFriendshipLevel(string email, int level)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(ConsumerQueries.GetConsumersByEmailAndFriendshipLevel, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@FriendshipLevel",level);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var consumers = reader.GetConsumersFromReader();
            return consumers;
        }

        public async Task<List<ConsumerModel>> GetConsumersByFriendEmail(string friendEmail)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(ConsumerQueries.GetConsumerByFriendEmail, connection);
            command.Parameters.AddWithValue("@Email", friendEmail);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var consumers= reader.GetConsumersFromReader();
            return consumers;

        }

        public async Task<List<ConsumerModel>> GetConsumersOptimised()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(ConsumerQueries.JoinWithNames, connection);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var consumers = reader.GetConsumersFromReader();

            return consumers;
        }

        public async Task<int> InsertManyConsumer(List<ConsumerModel> consumers)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateUser", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            connection.Open();

            int count = 0;
            foreach(var consumer in consumers)
            {
                command.CreateConsumerCommand(consumer);
                await command.ExecuteNonQueryAsync();
                count++;
            }

            return count;
        }

        public async Task<int> InsertManyFriend(List<ConsumerModel> consumerFriends)
        {
            List<ConsumerFriendModel> consumersModels = new List<ConsumerFriendModel>();
            foreach(var consumer in consumerFriends)
            {
                if (consumer.Friends != null)
                {
                    consumersModels.AddRange(consumer.Friends);
                }
            }
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateConsumerFriends", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandTimeout = 120;

            connection.Open();
            int count = 0;
            foreach (var consumerFriend in consumersModels)
            {
                command.CreateConsumerFriendCommand(consumerFriend);
                await command.ExecuteNonQueryAsync();
                count++;
            }

            return count;
        }

        public async Task InsertManyFriendBulk(List<ConsumerModel> consumerFriends)
        {
            List<ConsumerFriendModel> consumersModels = new List<ConsumerFriendModel>();
            foreach (var consumer in consumerFriends)
            {
                if (consumer.Friends != null)
                {
                    consumersModels.AddRange(consumer.Friends);
                }
            }
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateConsumerFriendsBulk", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();
            command.CreateConsumerFriendBulkCommand(consumersModels);

            await command.ExecuteNonQueryAsync();
            
        }

        public async Task InsertOneConsumer(ConsumerModel consumer)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateUser",connection);
            command.CommandType=System.Data.CommandType.StoredProcedure;

            connection.Open();
            command.CreateConsumerCommand(consumer);

            await command.ExecuteNonQueryAsync();
        }

        public async Task InsertOneFriend(ConsumerModel consumerFriend)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateConsumerFriends", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();
            command.CreateConsumerFriendCommand(consumerFriend.Friends.First());

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateConsumersByName(string name, int level)
        {
            string query = "UPDATE ConsumerFriends " +
                "SET friendshipLevel=@level " +
                "WHERE consumerId= IN (SELECT id FROM Consumers WHERE firstname=@name)";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@level", level);
            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateConsumersFriendshipById(long id, int level)
        {
            string query = "UPDATE ConsumerFriends " +
                "SET friendshipLevel=@level " +
                "WHERE consumerId= @id";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@level", level);
            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateConsumerTelephoneByEmail(string email, string telephone)
        {
            string query = "UPDATE Consumers= SET telephone=@telephone WHERE email=@email";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@email",email);
            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateConsumerTelephoneById(long consumerId, string telephone)
        {
            string query = "UPDATE Consumers= SET telephone=@telephone WHERE id=@consumerId";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@consumerId", consumerId);
            connection.Open();
            
            await command.ExecuteNonQueryAsync();
        }
    }
}
