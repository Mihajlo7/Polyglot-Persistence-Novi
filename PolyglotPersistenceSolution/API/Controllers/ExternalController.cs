using API.Services;
using Core.ExternalData;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Setup;
using SQLDataAccess;
using SQLDataAccess.impl;

namespace API.Controllers
{
    [ApiController]
    [Route("api/external/generate")]
    public class ExternalController : ControllerBase
    {
        private readonly IHttpClientFactoryService _httpClientFactoryService;
        //private readonly IConsumerRepository _consumerRepository;

        public ExternalController(IHttpClientFactoryService httpClientFactoryService)
        {
            _httpClientFactoryService = httpClientFactoryService;
            //_consumerRepository = consumerRepository;
        }

        [HttpGet("consumers")]
        public async Task<IActionResult> GetAllConsumers() 
        {
            
            var result = await _httpClientFactoryService.ExecuteAsync<ConsumerEx>("consumers",5);
            //ConsumerRepository consumerRepository = new();
            //int res= await consumerRepository.InsertConsumerBulk(result);
            var consumers= ConsumerGeneratorServicecs.GenerateConsumers(result);
            return Ok(consumers);
            
            //return Ok(await _consumerRepository.GetAllBySelect());
        }
        [HttpGet("consumersFriends/{num}")]
        public IActionResult GetConsumers(int num)
        {
            DatabaseAndDataSetupService ddss = new("");
            var res = ddss.GetConsumerFriends(20,250,null);
            if (num == 1)
            {
                return Ok(res.ToConsumersFromConsumersFriends().First());
            }
            else
            {
                return Ok(res.ToConsumersFromConsumersFriends());
            }
            
        }
        [HttpGet("sellers")]
        public async Task<IActionResult> GetAllSellers()
        {
            var sellers = await _httpClientFactoryService.ExecuteAsync<SellerEx>("sellers", 50);
            //CompanyRepository companyRepository = new();
            //int res= await companyRepository.InsertManySellers(sellers);
            return Ok(sellers);
        }

        [HttpGet("sellers2")]
        public  IActionResult GetAllSellers2()
        {
            //var sellers = await _httpClientFactoryService.ExecuteAsync<SellerEx>("sellers", 50);
            //CompanyRepository companyRepository = new();
            //int res= await companyRepository.InsertManySellers(sellers);
            DatabaseAndDataSetupService ddss = new("");
            var sellers=ddss.GetAllSellers();
            return Ok(sellers);
        }

        [HttpGet("cars")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _httpClientFactoryService.ExecuteAsync<CarEx>("cars", 50);
            //CompanyRepository companyRepository = new();
            //var sellers= await companyRepository.GetAllSellersBySelect();
            return Ok(cars);
        }
        [HttpGet("mobiles")]
        public async Task<IActionResult> GetAllMobiles()
        {
            var mobiles = await _httpClientFactoryService.ExecuteAsync<MobileEx>("mobiles", 50);
            //CompanyRepository companyRepository = new();
            //var sellers = await companyRepository.GetAllSellersBySelect();
            return Ok(mobiles);
        }

        [HttpGet("couriers")]
        public async Task<IActionResult> GetAllCouriers()
        {
            var couriers = await _httpClientFactoryService.ExecuteAsync<CourierEx>("couriers", 50);
            //CompanyRepository companyRepository = new();
            //var res=await companyRepository.InsertManyCouriers(couriers);
            return Ok(couriers);
        }

        [HttpGet("contracts")]
        public async Task<IActionResult> GetAllContracts()
        {
            CompanyRepository companyRepository = new();
            var couriers= await companyRepository.GetAllCouriersBySelect();
            var companies = await companyRepository.GetAllCompaniesBySelect();

            var results = CompanyGeneratorService.GenerateContractsForCourier(couriers, companies);

            return Ok(results);
        }

        [HttpGet("setup")]
        public IActionResult SetupDatabase()
        {
            DatabaseAndDataSetupService setupService= new DatabaseAndDataSetupService("small_db");
            setupService.SetupRelationDatabase();
            return Ok("Uspeli smo");
        }
        [HttpGet("products/{db}")]
        public IActionResult GetProducts(string db)
        {
            DatabaseAndDataSetupService ddss = new("");
            return Ok(ddss.GenerateProductsLarge());
        }
    }
}
