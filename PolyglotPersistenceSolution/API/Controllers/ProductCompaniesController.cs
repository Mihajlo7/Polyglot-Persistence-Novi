using Core.ExternalData;
using Microsoft.AspNetCore.Mvc;
using RelationDataAccess.Implementation;
using Services;
using Services.JsonWorker;

namespace API.Controllers
{
    [ApiController]
    [Route("api/products/companies")]
    public class ProductCompaniesController : ControllerBase
    {
        [HttpGet("insertSellers")]
        public async Task<IActionResult> InsertSellers()
        {
            JsonWorkerClass jsonWorker = new("Data");
            var sellersR = jsonWorker.ReadObjectsFromFile<SellerEx>("SellersRaw.json");
            var sellers = sellersR.ToSellers();

            SqlProductWithCompaniesRepository small_db = new("small_db");
            SqlProductWithCompaniesRepository medium_db = new("medium_db");
            SqlProductWithCompaniesRepository large_db = new("large_db");

            await small_db.InsertManySellers(sellers);
            await medium_db.InsertManySellers(sellers);
            await large_db.InsertManySellers(sellers);

            return Ok("Inserted sellers in all databases");
        }
    }
}
