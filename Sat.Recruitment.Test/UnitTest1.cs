using System;
using System.Dynamic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Interfaces;
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
            var mockUserValidator = new Mock<UserValidator>();

            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            FileStream fileStream = new FileStream(path, FileMode.Open);
            var mockUserReader = new Mock<IUserReader>();
            mockUserReader.Setup(
                x => x.ReadUsersFromFile())
                .Returns(() => new StreamReader(fileStream));

            var userService = new UserService(mockUserValidator.Object, mockUserReader.Object);
            var userController = new UsersController(userService);

            var result = userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");

            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }

        [Fact]
        public void Test1_2()
        {
            var mockService = new Mock<IUserService>();
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
            var mockService = new Mock<IUserService>();
            mockService.Setup(
                x => x.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124"))
                    .Returns(new Api.Models.Result { IsSuccess = false, Errors = "The user is duplicated" });

            var userController = new UsersController(mockService.Object);

            var result = userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");

            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Errors);
        }

        [Fact]
        public void Test3()
        {
            var mockUserValidator = new Mock<UserValidator>();

            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            FileStream fileStream = new FileStream(path, FileMode.Open);

            var mockUserReader = new Mock<IUserReader>();
            mockUserReader.Setup(
                x => x.ReadUsersFromFile())
                .Returns(() => new StreamReader(fileStream));

            var userService = new UserService(mockUserValidator.Object, mockUserReader.Object);

            var result = userService.CreateUser("", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");

            Assert.Equal("The name is required", result.Errors);
            Assert.False(result.IsSuccess);
            
        }

        [Fact]
        public void Test4()
        {
            var mockUserValidator = new Mock<UserValidator>();

            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            FileStream fileStream = new FileStream(path, FileMode.Open);

            var mockUserReader = new Mock<IUserReader>();
            mockUserReader.Setup(
                x => x.ReadUsersFromFile())
                .Returns(() => new StreamReader(fileStream));

            var userService = new UserService(mockUserValidator.Object, mockUserReader.Object);

            var result = userService.CreateUser("Agustina", "Agustina @gmail.com", "Garay y Otra Calle", "+534645213542", "SuperUser", "112234");

            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Errors);
        }

        [Fact]
        public void Test5()
        {
            var mockUserValidator = new Mock<UserValidator>();

            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            FileStream fileStream = new FileStream(path, FileMode.Open);

            var mockUserReader = new Mock<IUserReader>();
            mockUserReader.Setup(
                x => x.ReadUsersFromFile())
                .Returns(() => new StreamReader(fileStream));

            var userService = new UserService(mockUserValidator.Object, mockUserReader.Object);

            var result = userService.CreateUser("Agustina", "AgustinaR @gmail.com", "Garay y Otra Calle 2", "+534645213544", "SuperUser", "112234");

            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }
    }
}
