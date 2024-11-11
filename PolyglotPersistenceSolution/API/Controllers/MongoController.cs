using Core.Data.Mongo;
using Microsoft.AspNetCore.Mvc;
using MongoDataAccess;
using MongoDB.Driver;

namespace API.Controllers
{
    [ApiController]
    [Route("mongo")]
    public class MongoController : ControllerBase
    {
        ProductDataAccess mongo= new ProductDataAccess();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductWithCompany productWithCompany) 
        {
            await mongo.CreateProductWithCompany(productWithCompany);
            return Ok(productWithCompany);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await mongo.GetAllProductsWithCompany());
        }
    }
}
