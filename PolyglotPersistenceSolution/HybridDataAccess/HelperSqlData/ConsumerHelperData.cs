using Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HybridDataAccess.HelperSqlData
{
    internal static class ConsumerHelperData
    {
        public static SqlCommand CreateConsumerCommand(this SqlCommand command, ConsumerModel consumer)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Id", consumer.Id);
            command.Parameters.AddWithValue("@FirstName", consumer.FirstName);
            command.Parameters.AddWithValue("@LastName", consumer.LastName);
            command.Parameters.AddWithValue("@BirthDate", consumer.BirthDate);
            command.Parameters.AddWithValue("@Email", consumer.Email);
            command.Parameters.AddWithValue("@Telephone", consumer.Telephone);
            command.Parameters.AddWithValue("@Password", consumer.Password);
            command.Parameters.AddWithValue("@CreditCard1Id", consumer.CreditCards[0].Id);
            command.Parameters.AddWithValue("@CreditCardType1", consumer.CreditCards[0].CardType);
            command.Parameters.AddWithValue("@CreditCardNumber1", consumer.CreditCards[0].Number);
            command.Parameters.AddWithValue("@CreditCard2Id", consumer.CreditCards[1].Id);
            command.Parameters.AddWithValue("@CreditCardType2", consumer.CreditCards[1].CardType ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CreditCardNumber2", consumer.CreditCards[1].Number ?? (object)DBNull.Value);

            return command;
        }

        public static SqlCommand CreateConsumerFriendCommand(this SqlCommand command, ConsumerModel consumerFriend)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@ConsumerId", consumerFriend.Id);
            command.Parameters.AddWithValue("@FriendsData", JsonSerializer.Serialize(consumerFriend.Friends));

            return command;
        }

        public static SqlCommand CreateConsumerFriendsCommandBulk(this SqlCommand command,List<ConsumerModel> consumers)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ConsumerId",typeof(long));
            dataTable.Columns.Add("FriendsData", typeof(string));
            foreach (ConsumerModel consumer in consumers) 
            {
                dataTable.Rows.Add(consumer.Id,JsonSerializer.Serialize(consumer.Friends));
            }
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@FriendsDataType",dataTable);
            return command;
        }

        public static List<ConsumerModel> GetConsumersBadWay(this SqlDataReader reader)
        {
            List<ConsumerModel> consumers = new List<ConsumerModel>();

            while (reader.Read()) 
            {
                ConsumerModel consumer = new ConsumerModel()
                {
                    Id = reader.GetInt64(0),
                    Email = reader.GetString(1),
                    //2
                    //3
                    FirstName = reader.GetString(5),
                    LastName = reader.GetString(6),
                    BirthDate= reader.GetDateTime(7),
                    Telephone = reader.GetString(8),
                    Friends= JsonSerializer.Deserialize<List<ConsumerFriendModel>>(reader.GetString(9))
                };

                consumers.Add(consumer);
            }

            return consumers;
        }

        public static List<ConsumerModel> GetConsumers(this SqlDataReader reader)
        {
            List<ConsumerModel> consumers = new List<ConsumerModel>();

            while (reader.Read())
            {
                ConsumerModel consumer = new ConsumerModel()
                {
                    Id = reader.GetInt64("Id"),
                    Email = reader.GetString("Email"),
                    //2
                    //3
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    BirthDate = reader.GetDateTime("BirthDate"),
                    Telephone = reader.GetString("Telephone"),
                    Friends = JsonSerializer.Deserialize<List<ConsumerFriendModel>>(reader.GetString("Friends"))
                };

                consumers.Add(consumer);
            }

            return consumers;
        }

        
    }
}
