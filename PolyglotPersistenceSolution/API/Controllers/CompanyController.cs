using Core.Models;
using Microsoft.AspNetCore.Mvc;
using SQLDataAccess;

namespace API.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            return Ok(await _companyRepository.GetAllCompaniesBySelect());
        }

        [HttpGet("couriers")]
        public async Task<IActionResult> GetAllCouriers()
        {
            return Ok(await _companyRepository.GetAllCouriersBySelect());
        }

        [HttpPost("contract/insert-many")]
        public async Task<IActionResult> InsertManyCouriers([FromBody] List<ContractCourierModel> contracts)
        {
            return Ok(await _companyRepository.InsertManyContracts(contracts));
        }
    }
}
