using System;
using System.Dynamic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Services;
using Sat.Recruitment.Api.Utilities;
using Sat.Recruitment.Api.Validators;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var errors = "";
            var mockValidator = new Mock<UserValidator>();
            mockValidator.Setup(
                x => x.ValidateErrors("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", ref errors));

            var mockReader = new Mock<UserReader>();
            mockReader.Setup(
                x => x.ReadUsersFromFile())
                    .Returns(new StreamReader(new FileStream(Directory.GetCurrentDirectory() + "/Files/Users.txt", FileMode.Open)));

            var mockService = new Mock<UserService>(mockValidator, mockReader);
            mockService.Setup(
                x => x.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124"))
                    .Returns(new Api.Models.Result { IsSuccess = true, Errors = "User Created" });

            var userController = new UsersController(mockService.Object);

            var result = userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");

            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }

        [Fact]
        public void Test2()
        {
            var mockService = new Mock<UserService>();
            var userController = new UsersController(mockService.Object);

            var result = userController.CreateUser("Agustina", "Agustina@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");

            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Errors);
        }
    }
}
