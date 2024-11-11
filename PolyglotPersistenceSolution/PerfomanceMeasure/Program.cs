using BenchmarkDotNet.Running;
using PerfomanceMeasure.Consumer.Insert;
using PerfomanceMeasure.Consumer.Select;
using PerfomanceMeasure.ProductsWithSubcategory;

namespace PerfomanceMeasure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SelectConsumerHybridBenchMark>();
        }
    }
}
