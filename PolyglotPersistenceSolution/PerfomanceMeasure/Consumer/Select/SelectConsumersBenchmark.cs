using BenchmarkDotNet.Attributes;
using Core.Models;
using HybridDataAccess.Implementation;
using RelationDataAccess.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfomanceMeasure.Consumer.Select
{
    [SimpleJob(launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    public class SelectConsumersBenchmark
    {
        public SqlConsumerRepository small_db = new("small_db");
        public SqlConsumerRepository medium_db = new("medium_db");
        public SqlConsumerRepository large_db = new("large_db");

        

        

        [Benchmark]
        public async Task GetAllConsumersBadWaySmall()
        {
            await small_db.GetConsumers();
        }

        [Benchmark]
        public async Task GetAllConsumersOptimisedSmall()
        {
            await small_db.GetConsumersOptimised();
        }

        [Benchmark]
        public async Task GetAllConsumerByIdSmall()
        {
            await small_db.GetConsumerById(1_000_100);
        }

        [Benchmark]
        public async Task GetAllConsumerByEmailSmall()
        {
            await small_db.GetConsumerByEmail("creinert2n@time.com");
        }

        [Benchmark]
        public async Task GetAllConsumerByFriendEmailSmall()
        {
            await small_db.GetConsumersByFriendEmail("creinert2n@time.com");
        }

        [Benchmark]
        public async Task GetConsumersByEmailAndFriendshipLevelSmall()
        {
            await small_db.GetConsumersByEmailAndFriendshipLevel("creinert2n@time.com",5);
        }

        // Medium database benchmarks
        [Benchmark]
        public async Task GetAllConsumersBadWayMedium()
        {
            await medium_db.GetConsumers();
        }

        [Benchmark]
        public async Task GetAllConsumersOptimisedMedium()
        {
            await medium_db.GetConsumersOptimised();
        }

        [Benchmark]
        public async Task GetAllConsumerByIdMedium()
        {
            await medium_db.GetConsumerById(1_000_100);
        }

        [Benchmark]
        public async Task GetAllConsumerByEmailMedium()
        {
            await medium_db.GetConsumerByEmail("creinert2n@time.com");
        }

        [Benchmark]
        public async Task GetAllConsumerByFriendEmailMedium()
        {
            await medium_db.GetConsumersByFriendEmail("creinert2n@time.com");
        }

        [Benchmark]
        public async Task GetConsumersByEmailAndFriendshipLevelMedium()
        {
            await medium_db.GetConsumersByEmailAndFriendshipLevel("creinert2n@time.com", 5);
        }

        // Large database benchmarks
        [Benchmark]
        public async Task GetAllConsumersBadWayLarge()
        {
            await large_db.GetConsumers();
        }

        [Benchmark]
        public async Task GetAllConsumersOptimisedLarge()
        {
            await large_db.GetConsumersOptimised();
        }

        [Benchmark]
        public async Task GetAllConsumerByIdLarge()
        {
            await large_db.GetConsumerById(1_000_100);
        }

        [Benchmark]
        public async Task GetAllConsumerByEmailLarge()
        {
            await large_db.GetConsumerByEmail("creinert2n@time.com");
        }

        [Benchmark]
        public async Task GetAllConsumerByFriendEmailLarge()
        {
            await large_db.GetConsumersByFriendEmail("creinert2n@time.com");
        }

        [Benchmark]
        public async Task GetConsumersByEmailAndFriendshipLevelLarge()
        {
            await large_db.GetConsumersByEmailAndFriendshipLevel("creinert2n@time.com", 5);
        }
    }
}
