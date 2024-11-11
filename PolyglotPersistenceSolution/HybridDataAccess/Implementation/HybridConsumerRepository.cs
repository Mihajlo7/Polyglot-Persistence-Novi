using Core.ExternalData;
using Core.Models;
using HybridDataAccess.HelperSqlData;
using IDataAccess;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HybridDataAccess.Implementation
{
    public class HybridConsumerRepository : IConsumerRepository
    {
        private readonly string _connectionString;
        private readonly string _database;

        public HybridConsumerRepository(string database)
        {
            _database = database;
            _connectionString = $"Data Source=.;Initial Catalog={database};Integrated Security=True;TrustServerCertificate=True;";
        }

        public async Task<bool> DeleteConsumerById(long consumerId)
        {
            string query = "DELETE FROM Consumers WHERE Id=@consumerId;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@consumerId",consumerId);

            connection.Open();
            var res = await command.ExecuteNonQueryAsync();

            return res > 0;
        }

        public async Task<bool> DeleteConsumers()
        {
            string query = "DELETE FROM Consumers;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            var res = await command.ExecuteNonQueryAsync();

            return res > 0;
        }

        public async Task<bool> DeleteConsumersFriends()
        {
            string query = "UPDATE Consumers SET friends=NULL;";

            using var connection = new SqlConnection( _connectionString );
            using var command = new SqlCommand( query, connection );

            connection.Open();
            var res = await command.ExecuteNonQueryAsync();

            return res > 0;
        }

        public async Task<ConsumerModel> GetConsumerByEmail(string email)
        {
            string query = "SELECT u.id Id, u.email Email, c.firstName Firstname, c.lastName Lastname, c.birthDate BirthDate, c.telephone Telephone, c.friends Friends " +
                "\r\n  FROM Users u INNER JOIN Consumers c ON (u.id=c.id)" +
                "WHERE u.email=@Email";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetConsumers();

            return res.First();
        }

        public async Task<ConsumerModel> GetConsumerById(long id)
        {
            string query = "SELECT u.id Id, u.email Email, c.firstName Firstname, c.lastName Lastname, c.birthDate BirthDate, c.telephone Telephone, c.friends Friends " +
                "\r\n  FROM Users u INNER JOIN Consumers c ON (u.id=c.id)" +
                "WHERE u.id=@Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id",id);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetConsumers();

            return res.First();
        }

        public async Task<List<ConsumerModel>> GetConsumers()
        {
            string query = "SELECT * \r\n  FROM Users u INNER JOIN Consumers c ON (u.id=c.id)";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader= await command.ExecuteReaderAsync();
            command.CommandTimeout = 500;

            var res=reader.GetConsumersBadWay();

            return res;
        }

        public async Task<List<ConsumerModel>> GetConsumersByEmailAndFriendshipLevel(string email, int level)
        {
            string query = "SELECT u.id Id, u.email Email, c.firstName Firstname, c.lastName Lastname, c.birthDate BirthDate, c.telephone Telephone, c.friends,f.FriendshipLevel\r\nFROM Users u INNER JOIN Consumers c ON (u.id=c.id)\r\nCROSS APPLY OPENJSON(c.friends) WITH(\r\n\t--Friend NVARCHAR(MAX) AS JSON\r\n\tFriendshipLevel INT '$.FriendshipLevel'\r\n\t) as f\r\nWHERE u.email=@Email AND f.FriendshipLevel>@FriendshipLevel;";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@FriendshipLevel",level);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res= reader.GetConsumers();

            return res;
        }

        public async Task<List<ConsumerModel>> GetConsumersByFriendEmail(string friendEmail)
        {
            string query = "SELECT u.id Id, u.email Email, c.firstName Firstname, c.lastName Lastname, c.birthDate BirthDate, c.telephone Telephone ,c.friends Friends\r\nFROM Users u INNER JOIN Consumers c ON (u.id=c.id)\r\nCROSS APPLY OPENJSON(c.friends) WITH(\r\n\t--Friend NVARCHAR(MAX) AS JSON\r\n\tfriendEmail NVARCHAR(50) '$.Friend.Email'\r\n\t) as f\r\nWHERE f.friendEmail=@FriendEmail;";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FriendEmail",friendEmail);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            var res = reader.GetConsumers();
            return res;
        }

        public async Task<List<ConsumerModel>> GetConsumersOptimised()
        {
            string query = "SELECT u.id Id, u.email Email, c.firstName Firstname, c.lastName Lastname, c.birthDate BirthDate, c.telephone Telephone, c.friends Friends \r\n  FROM Users u INNER JOIN Consumers c ON (u.id=c.id)";
            
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.CommandTimeout = 500;
            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetConsumers();

            return res;
        }

        public async Task<int> InsertManyConsumer(List<ConsumerModel> consumers)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateUser", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            connection.Open();

            int count = 0;
            foreach (var consumer in consumers)
            {
                command.CreateConsumerCommand(consumer);
                await command.ExecuteNonQueryAsync();
                count++;
            }

            return count;
        }

        public async Task<int> InsertManyFriend(List<ConsumerModel> consumerFriends)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateFriendsJson", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            int count = 0;
            connection.Open();

            foreach (var consumer in consumerFriends)
            {
                command.CreateConsumerFriendCommand(consumer);
                await command.ExecuteNonQueryAsync();
                count++;
            }
            return count;
        }

        public async Task InsertManyFriendBulk(List<ConsumerModel> consumerFriends)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateFriendsJsonBulk", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            command.CreateConsumerFriendsCommandBulk(consumerFriends);

            await command.ExecuteNonQueryAsync();
        }

        public async Task InsertOneConsumer(ConsumerModel consumer)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateUser", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();
            command.CreateConsumerCommand(consumer);

            await command.ExecuteNonQueryAsync();
        }

        public async Task InsertOneFriend(ConsumerModel consumerFriend)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateFriendsJson", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();
            command.CreateConsumerFriendCommand(consumerFriend);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateConsumersByName(string name,int level)
        {
            string query = "SELECT u.id Id, u.email Email, c.firstName Firstname, c.lastName Lastname, c.birthDate BirthDate, c.telephone Telephone, c.friends Friends " +
                "\r\n  FROM Users u INNER JOIN Consumers c ON (u.id=c.id)" +
                "WHERE c.firstname=@name";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = await command.ExecuteReaderAsync();

            var res = reader.GetConsumers();

            foreach (var consumer in res) 
            {
                await UpdateConsumersFriendshipById(consumer.Id, level);
            }

        }

        public async Task UpdateConsumersFriendshipById(long id, int level)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("UpdateFriendshipLevelByConsumerId", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@ConsumerId", id);
            command.Parameters.AddWithValue("@FriendshipLevel", level);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateConsumerTelephoneByEmail(string email, string telephone)
        {
            string queryFriends = "WITH cte AS(" +
                "\r\n\tSELECT *\r\n\tFROM Consumers CROSS APPLY " +
                "\r\n\tOPENJSON(friends) f" +
                "\r\n\tWHERE JSON_VALUE(f.value,'$.Friend.Email')=@email\r\n)\r\n" +
                "UPDATE cte" +
                "\r\nSET friends = JSON_MODIFY(friends,'$['+ cte.[key]+ '].Friend.Telephone',@telephone)";

            string queryConsumer = "UPDATE Consumers SET telephone=@telephone WHERE id =(SELECT id FROM Users WHERE email=@email)";
            
            using var connection = new SqlConnection(_connectionString);
            using var commandFriends = new SqlCommand(queryFriends, connection);
            using var commandConsumer = new SqlCommand(queryConsumer, connection);

            commandFriends.Parameters.AddWithValue("@email",email);
            commandFriends.Parameters.AddWithValue("@telephone", telephone);

            commandConsumer.Parameters.AddWithValue("@email", email);
            commandConsumer.Parameters.AddWithValue("@telephone", telephone);

            connection.Open();

            await commandFriends.ExecuteNonQueryAsync();
            await commandConsumer.ExecuteNonQueryAsync();
        }

        public async Task UpdateConsumerTelephoneById(long consumerId, string telephone)
        {
            string queryFriends = "WITH cte AS(" +
                "\r\n\tSELECT *\r\n\tFROM Consumers CROSS APPLY " +
                "\r\n\tOPENJSON(friends) f" +
                "\r\n\tWHERE JSON_VALUE(f.value,'$.Friend.Id')=@id\r\n)\r\n" +
                "UPDATE cte" +
                "\r\nSET friends = JSON_MODIFY(friends,'$['+ cte.[key]+ '].Friend.Telephone',@telephone)";

            string queryConsumer = "UPDATE Consumers SET telephone=@telephone WHERE id =@id";

            using var connection = new SqlConnection(_connectionString);
            using var commandFriends = new SqlCommand(queryFriends, connection);
            using var commandConsumer = new SqlCommand(queryConsumer, connection);

            commandFriends.Parameters.AddWithValue("@id", consumerId);
            commandFriends.Parameters.AddWithValue("@telephone", telephone);

            commandConsumer.Parameters.AddWithValue("@id", consumerId);
            commandConsumer.Parameters.AddWithValue("@telephone", telephone);

            connection.Open();

            await commandFriends.ExecuteNonQueryAsync();
            await commandConsumer.ExecuteNonQueryAsync();
        }
    }
}
