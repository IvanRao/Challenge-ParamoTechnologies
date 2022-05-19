using Microsoft.AspNetCore.Mvc;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UnitTestController
    {
        [Fact]
        public void CreateUser_Return_Ok()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(
                x => x.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124"))
                    .Returns(new Api.Models.Result { IsSuccess = true, Errors = "User Created" });

            var userController = new UsersController(mockService.Object);

            // Act
            var actionResult = userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");
            var result = actionResult.Result as OkObjectResult;
            var returnResult = result.Value as Result;

            // Assert
            Assert.Equal("User Created", returnResult.Errors);
            Assert.True(returnResult.IsSuccess);
        }

        [Fact]
        public void CreateUser_Return_Duplicated()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(
                x => x.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124"))
                    .Returns(new Api.Models.Result { IsSuccess = false, Errors = "The user is duplicated" });

            var userController = new UsersController(mockService.Object);

            // Act
            var actionResult = userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");
            var result = actionResult.Result as BadRequestObjectResult;
            var returnResult = result.Value as Result;

            // Assert
            Assert.Equal("The user is duplicated", returnResult.Errors);
            Assert.False(returnResult.IsSuccess);
        }
    }
}
