using Core.Models;
using IDataAccess;
using Microsoft.AspNetCore.Mvc;
using RelationDataAccess.Implementation;
using Services.JsonWorker;

namespace API.Controllers
{
    [ApiController]
    [Route("api/subcategory/products")]
    public class ProductSubCategoryController : ControllerBase
    {
        private readonly IProductWithSubCategoryRepository _repository;

        public ProductSubCategoryController()
        {
            _repository = new SqlProductWithSubCategoryRepository("small_db");
        }
        [HttpGet("insertMany/{db}")]
        public async Task<IActionResult> InsertProducts(string db)
        {
            SqlProductWithSubCategoryRepository productWithSubCategoryRepository = new("small_db");

            JsonWorkerClass jsonWorker = new("Data");
            var products = jsonWorker.ReadObjectsFromFile<ProductModel>("Products_Small.json");

            int res=await productWithSubCategoryRepository.InsertMany(products);

            productWithSubCategoryRepository = new("medium_db");
            products= jsonWorker.ReadObjectsFromFile<ProductModel>("Products_Medium.json");
            res += await productWithSubCategoryRepository.InsertMany(products);

            productWithSubCategoryRepository = new("large_db");
            products = jsonWorker.ReadObjectsFromFile<ProductModel>("Products_Large.json");
            res += await productWithSubCategoryRepository.InsertMany(products);

            return Ok($"Inserted {res} products.");
        }

        [HttpPost("insertManyBulk")]
        public async Task<IActionResult> InsertManyBulk([FromBody] List<ProductModel> products)
        {
            //JsonWorkerClass jsonWorker = new("Data");
            //var products = jsonWorker.ReadObjectsFromFile<ProductModel>("Products_Small.json");

             await _repository.InsertManyBulk(products);

            return Ok($"Inserted bulk products");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAllProducts()
        {
            bool success = await _repository.DeleteAllProducts();
            if (success)
            {
                return Ok("Deleted all products");
            }
            else
            {
                return BadRequest("Unsuccefull delleting.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _repository.GetAllProducts());
        }

        [HttpGet("badw")]
        public async Task<IActionResult> GetAllProductsBadWay()
        {
            return Ok(await _repository.GetAllProductsBadWay());
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetAllProductById(long id)
        {
            return Ok(await _repository.GetProductById(id));
        }

        [HttpGet("subcategoryid/{id}")]
        public async Task<IActionResult> GetAllProductBySubcategoryId(long id)
        {
            return Ok(await _repository.GetProductsBySubCategoryId(id));
        }

        [HttpGet("subcategoryname/{name}")]
        public async Task<IActionResult> GetAllProductBySubcategoryId(string name)
        {
            return Ok(await _repository.GetProductsBySubCategoryName(name));
        }
    }
}
