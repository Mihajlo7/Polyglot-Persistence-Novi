using Core.Models;
using Microsoft.AspNetCore.Mvc;
using SQLDataAccess;

namespace API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository= productRepository;
        }
        [HttpPost("insert-one")]
        public async Task<IActionResult> InsertProduct([FromBody] ProductModel product)
        {
            return Ok(await _productRepository.InsertOne(product));
        }

        [HttpPost("insert-many")]
        public async Task<IActionResult> InsertProduct([FromBody] List<ProductModel> products)
        {
            return Ok(await _productRepository.InsertMany(products));
        }
        [HttpGet("select-join")]
        public async Task<IActionResult> GetProductsBySelect()
        {
            return Ok(await _productRepository.GetAllProductsWithCompaniesBySelect());
        }
        [HttpGet("select-join-opt")]
        public async Task<IActionResult> GetProductsBySelectOpt()
        {
            return Ok(await _productRepository.GetAllProductsWithCompaniesBySelectOptimised());
        }

        [HttpGet("select-subQuery")]
        public async Task<IActionResult> GetProductsBySelectSubQuery()
        {
            return Ok(await _productRepository.GetAllProductsWithCompaniesBySelectSubQuery());
        }
        [HttpGet("select/name/{name}")]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            return Ok (await _productRepository.GetProductsWithCompaniesByName(name));
        }

        [HttpGet("select/name-like/{name}")]
        public async Task<IActionResult> GetProductsByNameWithLike(string name)
        {
            return Ok(await _productRepository.GetProductsWithCompaniesByNameWithLike(name));
        }
        [HttpGet("select/id/{id}")]
        public async Task<IActionResult> GetProductsById(long id)
        {
            return Ok(await _productRepository.GetProductWithCompaniesById(id));
        }
        [HttpGet("select/produce-coutry/{produceCountry}/price/{price}")]
        public async Task<IActionResult> GetProductsWithCompaniesByProduceCountry(string produceCountry,decimal price)
        {
            return Ok(await _productRepository.GetProductsWithCompaniesByProduceCountryAndPrice(produceCountry,price));
        }

        [HttpGet("select/productName/{productName}/dCountry/{dCountry}/dPrice{dPrice}")]
        public async Task<IActionResult> GetProductsWithCompaniesByDistribute(string productName,string dCountry,decimal dPrice)
        {
            return Ok(await _productRepository.GetProductsWithCompaniesByNameAndDistributionCountryAndDistributionPrice(productName,dCountry,dPrice));
        }

        [HttpGet("details/join")]
        public async Task<IActionResult> GetProductsWithDetailsByJoin()
        {
            return Ok(await _productRepository.GetProductsWithDetailsByJoin());
        }

        [HttpGet("details/join/optimised")]
        public async Task<IActionResult> GetProductsWithDetailsByJoinOptimised()
        {
            return Ok(await _productRepository.GetProductsWithDetailsByJoinOptimised());
        }

        [HttpGet("details/subquery")]
        public async Task<IActionResult> GetProductsWithDetailsBySubQuery()
        {
            return Ok(await _productRepository.GetProductsWithDetailsBySubQuery());
        }

        [HttpGet("details/subquery/apply")]
        public async Task<IActionResult> GetProductsWithDetailsBySubQueryUsingApply()
        {
            return Ok(await _productRepository.GetProductsWithDetailsBySubQueryUsingApply());
        }

        [HttpGet("details/id/{productId}")]
        public async Task<IActionResult> GetProductWithDetailByProductId(long productId)
        {
            return Ok(await _productRepository.GetProductWithDetailByProductId(productId));
        }
    }
}
