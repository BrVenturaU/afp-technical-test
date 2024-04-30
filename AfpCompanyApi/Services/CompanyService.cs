using AfpCompanyApi.Data;
using AfpCompanyApi.Dtos;
using AfpCompanyApi.Exceptions;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AfpCompanyApi.Services;
public interface ICompanyService
{
    Task<CompanyDto> GetCompanyById(int id);
}

public class CompanyService: ICompanyService
{
    private readonly AppDbContext _context;

    public CompanyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyDto> GetCompanyById(int id)
    {
        var connection = _context.Connection;
        var company = await connection.QuerySingleOrDefaultAsync<CompanyDto>("SPGET_COMPANY_BY_ID", new { CompanyId = id },
            _context.Transaction, commandType: CommandType.StoredProcedure);
        if (company is null)
        {
            throw new CompanyNotFoundException(id);
        }
        return company;
    }
}
