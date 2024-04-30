using AfpCompanyApi.Controllers;
using AfpCompanyApi.Dtos;
using AfpCompanyApi.Exceptions;
using AfpCompanyApi.Models;
using AfpCompanyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AfpCompanyApi.Tests
{
    public class DepartmentsControllerTests
    {
        [Fact]
        public async Task Get_Departments_By_Company_Returns_Empty_List()
        {
            var mock = new Mock<IDepartmentService>();
            mock.Setup(service => service.GetDepartmentsByCompany(It.IsAny<int>()))
                .Returns(Task.FromResult(new List<DepartmentDto>().AsEnumerable()));

            var controller = new DepartmentsController(mock.Object);
            var response = await controller.GetDepartmentsByCompany(1);
            var result = response.Result as OkObjectResult;

            var departments = result.Value as IEnumerable<DepartmentDto>;
            Assert.NotNull(departments);
            Assert.Empty(departments);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Get_Departments_By_Company_Returns_List()
        {
            var departmentsDto = new List<DepartmentDto>()
            {
                new DepartmentDto
                {
                    Id = 1,
                    EmployeesCount = 1,
                    Level = OrganizationLevel.ESTRATEGICO,
                    Name = "Test Name"
                }
            };
            var mock = new Mock<IDepartmentService>();
            mock.Setup(service => service.GetDepartmentsByCompany(It.IsAny<int>()))
                .Returns(Task.FromResult(departmentsDto.AsEnumerable()));

            var controller = new DepartmentsController(mock.Object);
            var response = await controller.GetDepartmentsByCompany(1);
            var result = response.Result as OkObjectResult;

            var departments = result.Value as IEnumerable<DepartmentDto>;
            Assert.NotNull(departmentsDto);
            Assert.NotEmpty(departments);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Get_Departments_By_Company_Returns_NotFound_Company()
        {
            var departmentsDto = new List<DepartmentDto>()
            {
                new DepartmentDto
                {
                    Id = 1,
                    EmployeesCount = 1,
                    Level = OrganizationLevel.ESTRATEGICO,
                    Name = "Test Name"
                }
            };
            var mock = new Mock<IDepartmentService>();
            mock.Setup(service => service.GetDepartmentsByCompany(It.IsAny<int>()))
                .Throws(new CompanyNotFoundException(1));

            var controller = new DepartmentsController(mock.Object);
            var response = await controller.GetDepartmentsByCompany(1);
            var result = response.Result as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("was not found", result.Value.ToString());
        }
    }
}
