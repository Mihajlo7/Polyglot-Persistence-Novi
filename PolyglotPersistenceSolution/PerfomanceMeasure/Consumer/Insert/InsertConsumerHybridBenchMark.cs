using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Core.Models;
using HybridDataAccess.Implementation;
using RelationDataAccess.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfomanceMeasure.Consumer.Insert
{
    [SimpleJob(launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    public class InsertConsumerHybridBenchMark
    {
        public SqlConsumerRepository small_db = new("small_db");
        public SqlConsumerRepository medium_db = new("medium_db");
        public SqlConsumerRepository large_db = new("large_db");

        public HybridConsumerRepository hybrid_db = new("hybrid_small_db");

        public List<ConsumerModel> small_set;
        public List<ConsumerModel> medium_set;
        public List<ConsumerModel> large_set;

        [GlobalSetup ]
        public async Task GlobalUp()
        {
            small_set = await small_db.GetConsumersOptimised();
            medium_set= await medium_db.GetConsumersOptimised();
            large_set = await large_db.GetConsumersOptimised();
        }

        [IterationSetup]
        public void IterationSetup() 
        {
            hybrid_db.DeleteConsumersFriends();
        }

        [Benchmark]
        public async Task InsertOneSmall()
        {
           await hybrid_db.InsertOneFriend(small_set.First());
        }

        [Benchmark]
        public async Task InsertOneMedium()
        {
            await hybrid_db.InsertOneFriend(medium_set.First());
        }

        [Benchmark]
        public async Task InsertOneLarge()
        {
            await hybrid_db.InsertOneFriend(large_set.First());
        }

        [Benchmark]
        public async Task InsertManySmall()
        {
            await hybrid_db.InsertManyFriend(small_set);
        }

        [Benchmark]
        public async Task InsertManyMedium()
        {
            await hybrid_db.InsertManyFriend(medium_set);
        }

        [Benchmark]
        public async Task InsertManyLarge()
        {
            await hybrid_db.InsertManyFriend(large_set);
        }

        [Benchmark]
        public async Task InsertBulkSmall()
        {
            await hybrid_db.InsertManyFriendBulk(small_set);
        }

        [Benchmark]
        public async Task InsertBulkMedium()
        {
            await hybrid_db.InsertManyFriendBulk(medium_set);
        }

        [Benchmark]
        public async Task InsertBulkLarge()
        {
            await hybrid_db.InsertManyFriendBulk(large_set);
        }
    }
}
