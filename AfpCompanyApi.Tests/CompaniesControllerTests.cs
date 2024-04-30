using AfpCompanyApi.Controllers;
using AfpCompanyApi.Dtos;
using AfpCompanyApi.Exceptions;
using AfpCompanyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Moq;
using System.Net;

namespace AfpCompanyApi.Tests
{
    public class CompaniesControllerTests
    {
        [Fact]
        public async Task Get_Company_By_Id_Returns_Company()
        {
            var companyDto = new CompanyDto
            {
                Id = 1,
                Name = "Test Name",
                OfficialName = "Test OfficialName",
            };
            var mock = new Mock<ICompanyService>();
            mock.Setup(service => service.GetCompanyById(It.IsAny<int>())).Returns(Task.FromResult(companyDto));

            var controller = new CompaniesController(mock.Object);
            var response = await controller.GetCompanyById(1);
            var result = response.Result as OkObjectResult;

            var company = result.Value;
            Assert.NotNull(company);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Get_Company_By_Id_Returns_NotFound()
        {
            
            var mock = new Mock<ICompanyService>();
            mock.Setup(service => service.GetCompanyById(It.IsAny<int>()))
                .Throws(new CompanyNotFoundException(1));

            var controller = new CompaniesController(mock.Object);
            var response = await controller.GetCompanyById(1);
            var result = response.Result as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("was not found", result.Value.ToString());
        }
    }
}