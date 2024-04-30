using AfpCompanyApi.Dtos;
using AfpCompanyApi.Exceptions;
using AfpCompanyApi.Models;
using AfpCompanyApi.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AfpCompanyApi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController: ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyById(int id) {
            
            try
            {
                var some = await _companyService.GetCompanyById(id);
                return Ok(some);
            }catch (CompanyNotFoundException ex) {
                return NotFound(ex.Message);
            }
        }
    }
}
