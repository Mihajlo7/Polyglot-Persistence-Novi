using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace IDataAccess
{
    public interface IConsumerRepository
    {
        public Task InsertOneConsumer(ConsumerModel consumer);
        public Task<int> InsertManyConsumer(List<ConsumerModel> consumers);
        public Task InsertOneFriend(ConsumerModel consumerFriend);
        public Task<int> InsertManyFriend(List<ConsumerModel> consumerFriends);
        public Task InsertManyFriendBulk(List<ConsumerModel> consumerFriends);
        public Task<bool> DeleteConsumerById(long consumerId);
        public Task<bool> DeleteConsumers();
        public Task<bool> DeleteConsumersFriends();

        public Task<List<ConsumerModel>> GetConsumers();
        public Task<List<ConsumerModel>> GetConsumersOptimised();

        public Task<ConsumerModel> GetConsumerById(long id);
        public Task<ConsumerModel> GetConsumerByEmail(string email);
        public Task<List<ConsumerModel>> GetConsumersByFriendEmail(string friendEmail);
        public Task<List<ConsumerModel>> GetConsumersByEmailAndFriendshipLevel(string email, int level);

        public Task UpdateConsumerTelephoneById(long consumerId, string telephone);
        public Task UpdateConsumerTelephoneByEmail(string email, string telephone);
        public Task UpdateConsumersByName(string name,int level);
        public Task UpdateConsumersFriendshipById(long id, int level);
    }
}
