namespace AfpCompanyApi.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int EmployeesCount { get; set; }
    public OrganizationLevel Level { get; set; }
    public int CompanyId { get; set; }
}


public enum OrganizationLevel
{
    ESTRATEGICO,
    TACTICO,
    OPERATIVO
}