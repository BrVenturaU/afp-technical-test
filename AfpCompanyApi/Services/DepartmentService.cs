using AfpCompanyApi.Data;
using AfpCompanyApi.Dtos;
using AfpCompanyApi.Exceptions;
using AfpCompanyApi.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AfpCompanyApi.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetDepartmentsByCompany(int companyId);
}

public class DepartmentService: IDepartmentService
{
    private readonly AppDbContext _context;
    private readonly ICompanyService _companyService;

    public DepartmentService(AppDbContext context, ICompanyService companyService)
    {
        _context = context;
        _companyService = companyService;
    }

    public async Task<IEnumerable<DepartmentDto>> GetDepartmentsByCompany(int companyId)
    {
        var connection = _context.Connection;
        var company = await _companyService.GetCompanyById(companyId);
        
        var departments = await connection
            .QueryAsync<DepartmentDto>("SPGET_DEPARTMENTS_BY_COMPANY", new { CompanyId = company.Id },
            _context.Transaction, commandType: CommandType.StoredProcedure);

        return departments;
    }
}
