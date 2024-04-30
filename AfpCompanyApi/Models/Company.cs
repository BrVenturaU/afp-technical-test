namespace AfpCompanyApi.Models;

public class Company
{
    public int Id { get; set; }
    public string  Name { get; set; }
    public string OfficialName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}
