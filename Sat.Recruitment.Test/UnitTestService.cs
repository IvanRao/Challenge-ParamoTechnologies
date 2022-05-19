using Moq;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Services;
using Sat.Recruitment.Api.Validators;
using System.IO;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UnitTestService
    {
        [Fact]
        public void CreateUser_Returns_Ok()
        {
            // Arrange
            var UserValidator = new UserValidator();

            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            FileStream fileStream = new FileStream(path, FileMode.Open);

            var mockUserReader = new Mock<IUserReader>();
            mockUserReader.Setup(
                x => x.ReadUsersFromFile())
                .Returns(() => new StreamReader(fileStream));

            var userService = new UserService(UserValidator, mockUserReader.Object);

            // Act
            var result = userService.CreateUser("Agustina", "AgustinaR @gmail.com", "Garay y Otra Calle 2", "+534645213544", "SuperUser", "112234");

            // Assert
            Assert.Equal("User Created", result.Errors);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void CreateUser_Returns_NameValidation()
        {
            // Arrange
            var UserValidator = new UserValidator();

            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            FileStream fileStream = new FileStream(path, FileMode.Open);

            var mockUserReader = new Mock<IUserReader>();
            mockUserReader.Setup(
                x => x.ReadUsersFromFile())
                .Returns(() => new StreamReader(fileStream));

            var userService = new UserService(UserValidator, mockUserReader.Object);

            // Act
            var result = userService.CreateUser("", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");

            // Assert
            Assert.Equal("The name is required.", result.Errors);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CreateUser_Returns_PhoneNumberFormattingValidation()
        {
            // Arrange
            var UserValidator = new UserValidator();

            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            FileStream fileStream = new FileStream(path, FileMode.Open);

            var mockUserReader = new Mock<IUserReader>();
            mockUserReader.Setup(
                x => x.ReadUsersFromFile())
                .Returns(() => new StreamReader(fileStream));

            var userService = new UserService(UserValidator, mockUserReader.Object);

            // Act
            var result = userService.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 112 2354215", "Normal", "124");

            // Assert
            Assert.Equal("The phone number is incorrect.", result.Errors);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CreateUser_Returns_DuplicatedUser()
        {
            // Arrange
            var UserValidator = new UserValidator();

            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            FileStream fileStream = new FileStream(path, FileMode.Open);

            var mockUserReader = new Mock<IUserReader>();
            mockUserReader.Setup(
                x => x.ReadUsersFromFile())
                .Returns(() => new StreamReader(fileStream));

            var userService = new UserService(UserValidator, mockUserReader.Object);

            // Act
            var result = userService.CreateUser("Agustina", "Agustina @gmail.com", "Garay y Otra Calle", "+534645213542", "SuperUser", "112234");

            // Assert
            Assert.Equal("The user is duplicated", result.Errors);
            Assert.False(result.IsSuccess);
        }

        
    }
}
