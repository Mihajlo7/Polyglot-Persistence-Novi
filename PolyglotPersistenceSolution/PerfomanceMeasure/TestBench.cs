using BenchmarkDotNet.Attributes;
using SQLDataAccess.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfomanceMeasure
{
    [SimpleJob(launchCount: 1,warmupCount:2,iterationCount:6)]
    public class TestBench
    {
        private readonly ConsumerRepository consumerRepository=new();
        [Benchmark]
        public async Task Test()
        {
            await consumerRepository.GetAllBySelect();
        }
    }
}
