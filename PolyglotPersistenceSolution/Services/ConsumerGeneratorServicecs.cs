using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Core.ExternalData;
using Core.Models;

namespace Services
{
    public static class ConsumerGeneratorServicecs
    {
        private const int CONSUMER_INDEX = 1_000_000;
        private const int CREDIT_CARD = 2_000_000;

        public static List<ConsumerModel> GenerateConsumers(this List<ConsumerEx> consumersRaw)
        {
            int index = CONSUMER_INDEX;
            int creditCardIndex=CREDIT_CARD;
            List<ConsumerModel> consumers = new List<ConsumerModel>();
            foreach (ConsumerEx consumerRaw in consumersRaw)
            {
                ConsumerModel consumer = new ConsumerModel();
                consumer.FirstName = consumerRaw.FirstName;
                consumer.LastName = consumerRaw.LastName;
                consumer.Password = consumerRaw.Password;
                consumer.Email = consumerRaw.Email;
                consumer.Telephone = consumerRaw.Telephone;
                consumer.BirthDate = consumerRaw.BirthDay;
                consumer.Id = index++;
                consumer.CreditCards = new List<CreditCardModel>();
                consumer.CreditCards.Add(new CreditCardModel() { Id=creditCardIndex++,ConsumerId=consumer.Id,Number=consumerRaw.CreditCardNumber1,CardType=consumerRaw.CreditCardType1});
                if(consumerRaw.CreditCardNumber2 != null)
                {
                    consumer.CreditCards.Add(new CreditCardModel() { Id = creditCardIndex++, ConsumerId = consumer.Id, Number = consumerRaw.CreditCardNumber2, CardType = consumerRaw.CreditCardType2 });
                }
                consumers.Add(consumer);    
            }
            return consumers;
        }

        public static List<ConsumerFriendModel> GenerateConsumerFriends(this List<ConsumerModel> consumers,int numOfLinks,int numOfLinked)
        {
            List<ConsumerFriendModel> consumerFriends = new List<ConsumerFriendModel>();
            Random random = new Random();
            Dictionary<long, List<long>> lookUp = new();
            int done = 0;
            while (done < numOfLinked) 
            {
                
                int index;
                while (true)
                {
                    index=random.Next(CONSUMER_INDEX,CONSUMER_INDEX+5_000);
                    if (!lookUp.ContainsKey(index))
                    {
                        break;
                    }
                }
                bool initalized = false;
                for (int i = 0; i < numOfLinks; i++) 
                {
                    
                    int friendIndex;
                    while (true) 
                    {
                        friendIndex= random.Next(CONSUMER_INDEX, CONSUMER_INDEX + 5_000);
                        if (friendIndex != index)
                        {
                            if (initalized)
                            {
                                if (lookUp[index].Contains(friendIndex))
                                {
                                    continue;
                                }
                                else
                                {
                                    lookUp[index].Add(friendIndex);
                                    break;
                                }
                            }
                            else
                            {
                                lookUp[index] = new List<long> { friendIndex };
                                initalized = true;
                                break;
                            }
                        }
                    }
                }
                done++;
            }
            DateTime startDate = new DateTime(2020, 1, 1);  // Početni datum
            DateTime endDate = new DateTime(2025, 12, 31);  // Krajnji datum
            int range = (endDate - startDate).Days;
            foreach (var lu in lookUp)
            {
                var key=lu.Key;
                var values=lu.Value;
                for(int i = 0; i < values.Count; i++)
                {
                    ConsumerFriendModel consumerFriend = new();
                    consumerFriend.Id = key;
                    consumerFriend.Friend = consumers.Find(c => c.Id == values[i]);
                    consumerFriend.EstablishedDate = DateOnly.FromDateTime(startDate.AddDays(random.Next(range)));
                    consumerFriend.FriendshipLevel = random.Next(1, 10);
                    consumerFriends.Add(consumerFriend);
                }
            }
            return consumerFriends;
        }

        public static List<ConsumerModel> ToConsumersFromConsumersFriends(this List<ConsumerFriendModel> consumerFriends)
        {
            List<ConsumerModel> consumers = new List<ConsumerModel>();

            foreach (ConsumerFriendModel consumerFriend in consumerFriends)
            {
                var consumer = consumers.FirstOrDefault(c=>c.Id==consumerFriend.Id);
                if (consumer == null)
                {
                    consumer = new ConsumerModel();
                    consumer.Id = consumerFriend.Id;
                    consumer.Friends = new List<ConsumerFriendModel> { consumerFriend };
                    consumers.Add(consumer);
                }
                else
                {
                    consumer.Friends.Add(consumerFriend);
                }
            }

            return consumers;
        }
    }
}
