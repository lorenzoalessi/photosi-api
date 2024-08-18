using Moq;
using PhotosiApi.Dto.User;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients;
using PhotosiApi.Service.User;
using PhotosiApi.Service.User.Login;

namespace PhotosiApi.xUnitTest.Service;

public class UserServiceTest : TestSetup
{
    private readonly Mock<IBaseHttpClient> _mockBaseHttpClient;
    private readonly Mock<IUserLoginHandler> _mockUserLoginHandler;

    public UserServiceTest()
    {
        _mockUserLoginHandler = new Mock<IUserLoginHandler>();
        _mockBaseHttpClient = new Mock<IBaseHttpClient>();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnException_IfUserNotFounded()
    {
        // Arrange
        var service = GetService();
        var input = GenerateUserDto();

        // Act
        await Assert.ThrowsAsync<UserException>(() => service.RegisterAsync(input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Post<UserDto>(It.IsAny<string>(), input), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnObject_IfUserFounded()
    {
        // Arrange
        var service = GetService();
        var input = GenerateUserDto();

        _mockBaseHttpClient.Setup(x => x.Post<UserDto>(It.IsAny<string>(), input))
            .ReturnsAsync(input);

        // Act
        var result = await service.RegisterAsync(input);

        // Assert
        _mockBaseHttpClient.Verify(x => x.Post<UserDto>(It.IsAny<string>(), input), Times.Once);
        Assert.Equal(input, result);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_IfUserNotFounded()
    {
        // Arrange
        var service = GetService();
        var input = GenerateLoginDto();

        // Act
        await Assert.ThrowsAsync<UserException>(async () => await service.LoginAsync(input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Post<UserDto>(It.IsAny<string>(), input), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnObject_IfUserFounded()
    {
        // Arrange
        var service = GetService();
        var input = GenerateLoginDto();
        var userDto = GenerateUserDto();

        _mockBaseHttpClient.Setup(x => x.Post<UserDto>(It.IsAny<string>(), input))
            .ReturnsAsync(userDto);

        _mockUserLoginHandler.Setup(x => x.Users)
            .Returns([]);

        // Act
        var result = await service.LoginAsync(input);

        // Assert
        _mockBaseHttpClient.Verify(x => x.Post<UserDto>(It.IsAny<string>(), input), Times.Once);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetAsync_ShouldThrowException_IfUserNotFound()
    {
        // Arrange
        var service = GetService();

        // Act
        await Assert.ThrowsAsync<UserException>(async () => await service.GetAsync());

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<List<UserDto>>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnObject_IfUserFound()
    {
        // Arrange
        var service = GetService();
        var input = Enumerable.Range(0, _faker.Int(5, 15))
            .Select(x => GenerateUserDto())
            .ToList();

        _mockBaseHttpClient.Setup(x => x.Get<List<UserDto>>(It.IsAny<string>()))
            .ReturnsAsync(input);

        // Act
        var result = await service.GetAsync();

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<List<UserDto>>(It.IsAny<string>()), Times.Once);
        Assert.Equal(input, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_IfUserNotFound()
    {
        // Arrange
        var service = GetService();
        var input = _faker.Int(1);

        // Act
        await Assert.ThrowsAsync<UserException>(async () => await service.GetByIdAsync(input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<UserDto>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnObject_IfUserFound()
    {
        // Arrange
        var service = GetService();
        var id = _faker.Int(1);
        var input = GenerateUserDto();

        _mockBaseHttpClient.Setup(x => x.Get<UserDto>(It.IsAny<string>()))
            .ReturnsAsync(input);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.Equal(input, result);
        _mockBaseHttpClient.Verify(x => x.Get<UserDto>(It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task UpdateAsync_ReturnBool(bool updated)
    {
        // Assert
        var service = GetService();
        var id = _faker.Int(1);
        var input = GenerateUserDto();

        _mockBaseHttpClient.Setup(x => x.Put(It.IsAny<string>(), input))
            .ReturnsAsync(updated);

        // Act
        var result = await service.UpdateAsync(id, input);

        // Arrange
        Assert.Equal(updated, result);
        _mockBaseHttpClient.Verify(x => x.Put(It.IsAny<string>(), input), Times.Once);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task DeleteAsync_ReturnBool(bool updated)
    {
        // Assert
        var service = GetService();
        var id = _faker.Int(1);

        _mockBaseHttpClient.Setup(x => x.Delete(It.IsAny<string>()))
            .ReturnsAsync(updated);

        // Act
        var result = await service.DeleteAsync(id);

        // Arrange
        Assert.Equal(updated, result);
        _mockBaseHttpClient.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
    }

    private IUserService GetService()
    {
        return new UserService(_mockAppSettings.Object, _mockUserLoginHandler.Object, _mockBaseHttpClient.Object);
    }

    private LoginDto GenerateLoginDto()
    {
        return new LoginDto
        {
            Username = _faker.String2(15, 30),
            Password = _faker.String2(15, 30)
        };
    }

    private UserDto GenerateUserDto()
    {
        return new UserDto
        {
            Id = _faker.Int(1),
            FirstName = _faker.String2(15, 30),
            LastName = _faker.String2(15, 30),
            Username = _faker.String2(15, 30),
            Password = _faker.String2(15, 30),
            Email = _faker.String2(15, 30),
            BirthDate = GenerateRandomDate()
        };
    }

    private DateTime GenerateRandomDate()
    {
        // Genero un anno casuale
        var year = _faker.Int(1950, DateTime.Now.Year);
        // Genero un mese casuale
        var month = _faker.Int(1, 12);
        // Genero un giorno casuale
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var day = _faker.Int(1, daysInMonth);

        return new DateTime(year, month, day);
    }
}