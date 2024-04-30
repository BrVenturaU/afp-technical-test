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
                return await _companyService.GetCompanyById(id);
            }catch (CompanyNotFoundException ex) {
                return NotFound(ex.Message);
            }
        }
    }
}
