using BenchmarkDotNet.Attributes;
using HybridDataAccess.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfomanceMeasure.Consumer.Select
{
    [SimpleJob(launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    public class SelectConsumerHybridBenchMark
    {
        public HybridConsumerRepository large_db = new("hybrid_small_db");

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
