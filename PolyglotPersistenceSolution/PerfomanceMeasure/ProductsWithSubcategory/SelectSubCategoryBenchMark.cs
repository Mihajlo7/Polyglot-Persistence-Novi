using BenchmarkDotNet.Attributes;
using RelationDataAccess.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfomanceMeasure.ProductsWithSubcategory
{
    [SimpleJob(launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    public class SelectSubCategoryBenchMark
    {
        public SqlProductWithSubCategoryRepository small_db = new("small_db");
        public SqlProductWithSubCategoryRepository medium_db = new("medium_db");
        public SqlProductWithSubCategoryRepository large_db = new("large_db");

        [Benchmark]
        public async Task SelectBadWaySmall()
        {
            await small_db.GetAllProductsBadWay();
        }
        [Benchmark]
        public async Task SelectAllSmall()
        {
            await small_db.GetAllProducts();
        }
        [Benchmark]
        public async Task GetProductByIdSmall()
        {
            await small_db.GetProductById(4_000_038);
        }
        [Benchmark]
        public async Task GetProductsBySubcategoryId()
        {
            await small_db.GetProductsBySubCategoryId(11);
        }
        [Benchmark]
        public async Task GetProductsBySubcategoryName()
        {
            await small_db.GetProductsBySubCategoryName("Toyota");
        }

        // Medium database benchmarks
        [Benchmark]
        public async Task SelectBadWayMedium()
        {
            await medium_db.GetAllProductsBadWay();
        }

        [Benchmark]
        public async Task SelectAllMedium()
        {
            await medium_db.GetAllProducts();
        }

        [Benchmark]
        public async Task GetProductByIdMedium()
        {
            await medium_db.GetProductById(4_000_038);
        }

        [Benchmark]
        public async Task GetProductsBySubcategoryIdMedium()
        {
            await medium_db.GetProductsBySubCategoryId(11);
        }

        [Benchmark]
        public async Task GetProductsBySubcategoryNameMedium()
        {
            await medium_db.GetProductsBySubCategoryName("Toyota");
        }

        // Large database benchmarks
        [Benchmark]
        public async Task SelectBadWayLarge()
        {
            await large_db.GetAllProductsBadWay();
        }

        [Benchmark]
        public async Task SelectAllLarge()
        {
            await large_db.GetAllProducts();
        }

        [Benchmark]
        public async Task GetProductByIdLarge()
        {
            await large_db.GetProductById(4_000_038);
        }

        [Benchmark]
        public async Task GetProductsBySubcategoryIdLarge()
        {
            await large_db.GetProductsBySubCategoryId(11);
        }

        [Benchmark]
        public async Task GetProductsBySubcategoryNameLarge()
        {
            await large_db.GetProductsBySubCategoryName("Toyota");
        }
    }
}
