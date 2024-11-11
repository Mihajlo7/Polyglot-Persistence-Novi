using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Core.Models;
using RelationDataAccess.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfomanceMeasure.ProductsWithSubcategory
{
    [SimpleJob( launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    public class InsertSubCategoryBenchmark
    {

        public SqlProductWithSubCategoryRepository small_db= new("small_db");
        public SqlProductWithSubCategoryRepository medium_db = new("medium_db");
        public SqlProductWithSubCategoryRepository large_db = new("large_db");

        public List<ProductModel> small_set;
        public List<ProductModel> medium_set;
        public List<ProductModel> large_set;
        public bool init=false;
        [GlobalSetup]
        public async Task GlobalSetUp()
        {
            
                small_set= await small_db.GetAllProducts();
                medium_set= await  medium_db.GetAllProducts();
                large_set= await large_db.GetAllProducts();
                init=true;
            
            
            
        }
        [IterationSetup]
        public void IterationSetup()
        {
            small_db.DeleteAllProducts();
            medium_db.DeleteAllProducts();
            large_db.DeleteAllProducts();
        }

        [IterationCleanup]
        public void IterationCleanUp()
        {
            small_db.DeleteAllProducts();
            medium_db.DeleteAllProducts();
            large_db.DeleteAllProducts();
        }
        [Benchmark]
        public async Task InsertOneSmalldb()
        {
            await small_db.InsertOne(small_set.First());
        }
        [Benchmark]
        public async Task InsertOneMediumldb()
        {
            await medium_db.InsertOne(medium_set.First());
        }
        [Benchmark]
        public async Task InsertOneLargedb()
        {
            await large_db.InsertOne(large_set.First());
        }

        [Benchmark]
        public async Task InsertManySmalldb()
        {
            await small_db.InsertMany(small_set);
        }
        [Benchmark]
        public async Task InsertManyMediumdb()
        {
            await medium_db.InsertMany(medium_set);
        }

        [Benchmark]
        public async Task InsertManyLargedb()
        {
            await large_db.InsertMany(large_set);
        }

        [Benchmark]
        public async Task InsertBulkSmalldb()
        {
            await small_db.InsertManyBulk(small_set);
        }

        [Benchmark]
        public async Task InsertBulkMediumdb()
        {
            await medium_db.InsertManyBulk(medium_set);
        }

        [Benchmark]
        public async Task InsertBulkLargedb()
        {
            await large_db.InsertManyBulk(large_set);
        }

        [GlobalCleanup]
        public async Task GlobalCleanUp()
        {
            await small_db.InsertManyBulk(small_set);
            await medium_db.InsertManyBulk(medium_set);
            await large_db.InsertManyBulk(large_set);
        }
    }
}
