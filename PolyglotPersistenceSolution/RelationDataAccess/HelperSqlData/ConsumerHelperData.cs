using Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationDataAccess.HelperSqlData
{
    public static class ConsumerHelperData
    {
        public static SqlCommand CreateConsumerCommand(this SqlCommand command,ConsumerModel consumer) 
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

        public static SqlCommand CreateConsumerFriendCommand(this SqlCommand command,ConsumerFriendModel consumerFriend)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@ConsumerId",consumerFriend.Id);
            command.Parameters.AddWithValue("@FriendId", consumerFriend.Friend.Id);
            command.Parameters.AddWithValue("@FriendshipLevel", consumerFriend.FriendshipLevel);
            command.Parameters.AddWithValue("@EstablishedDate",consumerFriend.EstablishedDate);

            return command;
        }
        public static SqlCommand CreateConsumerFriendBulkCommand(this  SqlCommand command, List<ConsumerFriendModel> consumerFriendList) 
        {
            DataTable table = new DataTable();
            table.Columns.Add("consumerId",typeof(long));
            table.Columns.Add("friendId",typeof(long));
            table.Columns.Add("friendshipLevel",typeof(int));
            table.Columns.Add("establishedDate",typeof(DateTime));

            foreach (var consumerFriend in consumerFriendList) 
            {
                table.Rows.Add(consumerFriend.Id,consumerFriend.Friend.Id,consumerFriend.FriendshipLevel,consumerFriend.EstablishedDate.ToDateTime(TimeOnly.MinValue));
            }

            command.Parameters.AddWithValue("@FriendsData",table);
            return command;
        }

        public static List<ConsumerModel> GetConsumersFromReaderBadWay(this SqlDataReader reader) 
        {
            List<ConsumerModel> consumers = new();

            while (reader.Read())
            {
                var consumerId = reader.GetInt64(0);

                // Pronađi ili dodaj novog consumer-a u listu
                var consumer = consumers.FirstOrDefault(c => c.Id == consumerId);
                if (consumer == null)
                {
                    // Kreiraj novog consumer-a
                    consumer = new ConsumerModel
                    {
                        Id = consumerId,
                        Email = reader.GetString(1),
                        FirstName = reader.GetString(5),
                        LastName = reader.GetString(6),
                        BirthDate = reader.GetDateTime(7),
                        Telephone = reader.GetString(8),
                        CreditCards = new List<CreditCardModel>(),
                        Friends = new List<ConsumerFriendModel>()
                    };
                    consumers.Add(consumer);
                }

                // Dodaj kreditnu karticu ako ne postoji
                var creditCardId = reader.GetInt64(9);
                if (!consumer.CreditCards.Any(cc => cc.Id == creditCardId))
                {
                    consumer.CreditCards.Add(new CreditCardModel
                    {
                        Id = creditCardId,
                        ConsumerId = reader.GetInt64(10),
                        Number = reader.GetString(11),
                        CardType = reader.GetString(12),
                    });
                }

                // Dodaj prijatelja ako postoji u podacima i nije već dodat
                if (!reader.IsDBNull(13))
                {
                    var friendId = reader.GetInt64(14);
                    if (!consumer.Friends.Any(cf => cf.Friend.Id == friendId))
                    {
                        ConsumerFriendModel consumerFriend = new ConsumerFriendModel
                        {
                            Id = reader.GetInt64(13),
                            FriendshipLevel = reader.GetInt32(15),
                            EstablishedDate = DateOnly.FromDateTime(reader.GetDateTime(16)),
                            Friend = new ConsumerModel
                            {
                                Id = friendId,
                                Email = reader.GetString(18),
                                FirstName = reader.GetString(19),
                                LastName = reader.GetString(20),
                                BirthDate = reader.GetDateTime(21),
                                Telephone = reader.GetString(22)
                            }
                        };
                        consumer.Friends.Add(consumerFriend);
                    }
                }
            }
            return consumers;
        }

        public static List<ConsumerModel> GetConsumersFromReader(this SqlDataReader reader)
        {
            List<ConsumerModel> consumers = new();

            while (reader.Read())
            {
                var consumerId = reader.GetInt64("Id");

                // Pronađi ili dodaj novog consumer-a u listu
                var consumer = consumers.FirstOrDefault(c => c.Id == consumerId);
                if (consumer == null)
                {
                    // Kreiraj novog consumer-a
                    consumer = new ConsumerModel
                    {
                        Id = consumerId,
                        Email = reader.GetString("Email"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        BirthDate = reader.GetDateTime("BirthDate"),
                        Telephone = reader.GetString("Telephone"),
                        CreditCards = new List<CreditCardModel>(),
                        Friends = new List<ConsumerFriendModel>()
                    };
                    consumers.Add(consumer);
                }

                // Dodaj kreditnu karticu ako ne postoji
                var creditCardId = reader.GetInt64("CreditCardId");
                if (!consumer.CreditCards.Any(cc => cc.Id == creditCardId))
                {
                    consumer.CreditCards.Add(new CreditCardModel
                    {
                        Id = creditCardId,
                        ConsumerId = consumerId,
                        Number = reader.GetString("CreditCardNumber"),
                        CardType = reader.GetString("CreditCardType"),
                    });
                }

                // Dodaj prijatelja ako postoji u podacima i nije već dodat
                if (!reader.IsDBNull(reader.GetOrdinal("FriendId")))
                {
                    var friendId = reader.GetInt64("FriendId");
                    if (!consumer.Friends.Any(cf => cf.Friend.Id == friendId))
                    {
                        ConsumerFriendModel consumerFriend = new ConsumerFriendModel
                        {
                            Id = consumerId,
                            FriendshipLevel = reader.GetInt32("FriendshipLevel"),
                            EstablishedDate = DateOnly.FromDateTime(reader.GetDateTime("FriendEstablishedDate")),
                            Friend = new ConsumerModel
                            {
                                Id = friendId,
                                Email = reader.GetString("friendEmail"),
                                FirstName = reader.GetString("friendName"),
                                LastName = reader.GetString("friendLastName"),
                                BirthDate = reader.GetDateTime("FriendBirthDate"),
                                Telephone = reader.GetString("FriendTelephone")
                            }
                        };
                        consumer.Friends.Add(consumerFriend);
                    }
                }
            }
            return consumers;
        }
    }
}
