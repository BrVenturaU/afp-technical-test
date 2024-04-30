using AfpCompanyApi.Models;
using System.Text.Json.Serialization;

namespace AfpCompanyApi.Dtos
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmployeesCount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrganizationLevel Level { get; set; }
    }
}
