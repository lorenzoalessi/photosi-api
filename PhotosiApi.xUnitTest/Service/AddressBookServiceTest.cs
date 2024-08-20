using Moq;
using PhotosiApi.Dto.AddressBook;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients;
using PhotosiApi.Service.AddressBook;

namespace PhotosiApi.xUnitTest.Service;

public class AddressBookServiceTest : TestSetup
{
    private readonly Mock<IBaseHttpClient> _mockBaseHttpClientMock;

    public AddressBookServiceTest()
    {
        _mockBaseHttpClientMock = new Mock<IBaseHttpClient>();
    }

    [Fact]
    public async Task GetAsync_ShouldThrowException_IfNoAddressFounded()
    {
        // Arrange
        var service = GetService();

        _mockBaseHttpClientMock.Setup(x => x.Get<List<AddressBookDto>>(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        // Act
        await Assert.ThrowsAsync<AddressBookException>(async () => await service.GetAsync());

        // Assert
        _mockBaseHttpClientMock.Verify(x => x.Get<List<AddressBookDto>>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnAddressesBook_IfFounded()
    {
        // Arrange
        var service = GetService();

        var addressesBook = Enumerable.Range(0, _faker.Int(15, 30))
            .Select(x => GenerateAddressBookDto())
            .ToList();

        _mockBaseHttpClientMock.Setup(x => x.Get<List<AddressBookDto>>(It.IsAny<string>()))
            .ReturnsAsync(addressesBook);

        // Act
        var result = await service.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count == addressesBook.Count);
        Assert.Empty(result.Select(x => x.Id).Except(addressesBook.Select(x => x.Id)));
        _mockBaseHttpClientMock.Verify(x => x.Get<List<AddressBookDto>>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_IfAddressBookNotFound()
    {
        // Arrange
        var service = GetService();
        var input = _faker.Int(1);

        _mockBaseHttpClientMock.Setup(x => x.Get<List<AddressBookDto>>(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        // Act
        await Assert.ThrowsAsync<AddressBookException>(async () => await service.GetByIdAsync(input));

        // Assert
        _mockBaseHttpClientMock.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAddressesBook_IfFounded()
    {
        // Arrange
        var service = GetService();
        var input = _faker.Int(1);

        var addressesBook = GenerateAddressBookDto();

        _mockBaseHttpClientMock.Setup(x => x.Get<AddressBookDto>(It.IsAny<string>()))
            .ReturnsAsync(addressesBook);

        // Act
        var result = await service.GetByIdAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id == addressesBook.Id);
        Assert.True(result.Cap == addressesBook.Cap);
        Assert.True(result.CityName == addressesBook.CityName);
        Assert.True(result.AddressNumber == addressesBook.AddressNumber);
        Assert.True(result.AddressName == addressesBook.AddressName);
        Assert.True(result.CountryName == addressesBook.CountryName);
        _mockBaseHttpClientMock.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateAsync_ShouldReturnBool_IfUpdateOkOrNot(bool updateOk)
    {
        // Arrange
        var service = GetService();
        var id = _faker.Int(1);
        var input = GenerateAddressBookDto(id);

        _mockBaseHttpClientMock.Setup(x => x.Put(It.IsAny<string>(), It.IsAny<AddressBookDto>()))
            .ReturnsAsync(updateOk);

        // Act
        var result = await service.UpdateAsync(id, input);

        // Assert
        Assert.Equal(updateOk, result);
        _mockBaseHttpClientMock.Verify(x => x.Put(It.IsAny<string>(), It.IsAny<AddressBookDto>()), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteAsync_ShouldReturnBool_IfDeleteOkOrNot(bool deleteOk)
    {
        // Arrange
        var service = GetService();
        var id = _faker.Int(1);

        _mockBaseHttpClientMock.Setup(x => x.Delete(It.IsAny<string>()))
            .ReturnsAsync(deleteOk);

        // Act
        var result = await service.DeleteAsync(id);

        // Assert
        Assert.Equal(deleteOk, result);
        _mockBaseHttpClientMock.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_IfPostFailed()
    {
        // Arrange
        var service = GetService();
        var input = GenerateAddressBookDto();

        _mockBaseHttpClientMock.Setup(x => x.Post<AddressBookDto>(It.IsAny<string>(), input))
            .ReturnsAsync(() => null);

        // Act
        await Assert.ThrowsAsync<AddressBookException>(async () => await service.AddAsync(input));

        // Assert
        _mockBaseHttpClientMock.Verify(x => x.Post<AddressBookDto>(It.IsAny<string>(), input), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnObject_IfPostOk()
    {
        // Arrange
        var service = GetService();
        var input = GenerateAddressBookDto();

        _mockBaseHttpClientMock.Setup(x => x.Post<AddressBookDto>(It.IsAny<string>(), input))
            .ReturnsAsync(input);

        // Act
        var result = await service.AddAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(input.Id, result.Id);
        Assert.Equal(input.Cap, result.Cap);
        Assert.Equal(input.AddressName, result.AddressName);
        Assert.Equal(input.CountryName, result.CountryName);
        Assert.Equal(input.AddressNumber, result.AddressNumber);
        Assert.Equal(input.CityName, result.CityName);
        _mockBaseHttpClientMock.Verify(x => x.Post<AddressBookDto>(It.IsAny<string>(), input), Times.Once);
    }

    private IAddressBookService GetService()
    {
        return new AddressBookService(_mockAppSettings.Object, _mockBaseHttpClientMock.Object);
    }

    private AddressBookDto GenerateAddressBookDto(int? id = null)
    {
        return new AddressBookDto
        {
            Id = id ?? _faker.Int(1),
            AddressName = _faker.String2(15, 30),
            AddressNumber = _faker.String2(15, 30),
            Cap = _faker.String2(15, 30),
            CityName = _faker.String2(15, 30),
            CountryName = _faker.String2(15, 30)
        };
    }
}