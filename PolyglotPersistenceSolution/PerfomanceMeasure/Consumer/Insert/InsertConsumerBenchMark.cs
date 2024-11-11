using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Core.Models;
using RelationDataAccess.Implementation;
using Services;
using Services.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfomanceMeasure.Consumer.Insert
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    public class InsertConsumerBenchMark
    {
        private  SqlConsumerRepository _smallDb;
        private  SqlConsumerRepository _mediumDb;
        private  SqlConsumerRepository _largeDb;

        DatabaseAndDataSetupService ddss = new("large_db");
        List<ConsumerModel> smallList;
        List<ConsumerModel>mediumList;
        List<ConsumerModel> largeList;

        [GlobalSetup]
        public async Task Setup()
        {
            _smallDb = new("small_db");
            _mediumDb = new("medium_db");
            _largeDb = new("large_db");
            List<ConsumerModel> consumers= await _smallDb.GetConsumersOptimised();
            smallList = ddss.GetConsumerFriends(20,250,consumers).ToConsumersFromConsumersFriends();
            mediumList = ddss.GetConsumerFriends(50, 1000, consumers).ToConsumersFromConsumersFriends();
            largeList = ddss.GetConsumerFriends(100, 5000, consumers).ToConsumersFromConsumersFriends();
        }


        [IterationSetup]
        public void IterationSetup()
        {
           // _smallDb.DeleteConsumersFriends();
            _mediumDb.DeleteConsumersFriends();
            //_largeDb.DeleteConsumersFriends();
        }
        /*
        [Benchmark]
        public async Task InsertOneSmall()
        {
            await _smallDb.InsertOneFriend(smallList.First());
        }

        [Benchmark]
        public async Task InsertOneMedium()
        {
            await _mediumDb.InsertOneFriend(mediumList.First());
            
        }

        [Benchmark]
        public async Task InsertOneLarge()
        {
            await _largeDb.InsertOneFriend(largeList.First());
           
        }

        [Benchmark]
        public async Task InsertManySmall()
        {
            await _smallDb.InsertManyFriend(smallList);
            
        }
        */
        [Benchmark]
        public async Task InsertManyMedium()
        {
            await _mediumDb.InsertManyFriend(mediumList);
            
        }
    
       // [Benchmark]
        public async Task InsertManyLarge()
        {
            await _largeDb.InsertManyFriend(largeList);
            
        }
/*
        [Benchmark]
        public async Task InsertBulkSmall()
        {
            await _smallDb.InsertManyFriendBulk(smallList);
            
        }
        [Benchmark]
        public async Task InsertBulkMedium()
        {
            await _mediumDb.InsertManyFriendBulk(mediumList);
           
        }

        [Benchmark]
        public async Task InsertBulkLarge()
        {
            await _largeDb.InsertManyFriendBulk(largeList);
            
        }
*/
    }
}
