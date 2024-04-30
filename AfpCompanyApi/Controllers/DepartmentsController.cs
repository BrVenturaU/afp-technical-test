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
    [Route("api/companies/{companyId:int}/departments")]
    public class DepartmentsController: ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartmentsByCompany(int companyId)
        {
            try
            {
                var departments = await _departmentService.GetDepartmentsByCompany(companyId);
                return Ok(departments);
            }
            catch (CompanyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
