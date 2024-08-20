using Moq;
using PhotosiApi.Dto.Product;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients;
using PhotosiApi.Service.Product;

namespace PhotosiApi.xUnitTest.Service;

public class ProductServiceTest : TestSetup
{
    private readonly Mock<IBaseHttpClient> _mockBaseHttpClient;

    public ProductServiceTest()
    {
        _mockBaseHttpClient = new Mock<IBaseHttpClient>();
    }

    [Fact]
    public async Task GetAsync_ShouldThrowException_IfProductNotFound()
    {
        // Arrange
        var service = GetService();

        // Act
        await Assert.ThrowsAsync<ProductException>(async () => await service.GetAsync());

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<List<ProductDto>>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnObject_IfProductFound()
    {
        // Arrange
        var service = GetService();
        var productsDto = Enumerable.Range(0, _faker.Int(15, 30))
            .Select(x => GetProductDto())
            .ToList();

        _mockBaseHttpClient.Setup(x => x.Get<List<ProductDto>>(It.IsAny<string>()))
            .ReturnsAsync(productsDto);

        // Act
        var result = await service.GetAsync();

        // Assert
        Assert.Equal(productsDto, result);
        _mockBaseHttpClient.Verify(x => x.Get<List<ProductDto>>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_IfProductNotFound()
    {
        // Arrange
        var service = GetService();
        var id = _faker.Int(1);

        // Act
        await Assert.ThrowsAsync<ProductException>(async () => await service.GetByIdAsync(id));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<ProductDto>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnObject_IfProductFound()
    {
        // Arrange
        var service = GetService();
        var id = _faker.Int(1);
        var productDto = GetProductDto();

        _mockBaseHttpClient.Setup(x => x.Get<ProductDto>(It.IsAny<string>()))
            .ReturnsAsync(productDto);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.Equal(productDto, result);
        _mockBaseHttpClient.Verify(x => x.Get<ProductDto>(It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateAsync_ShouldReturnBool(bool updated)
    {
        // Arrange
        var service = GetService();
        var id = _faker.Int(1);
        var productDto = GetProductDto();

        _mockBaseHttpClient.Setup(x => x.Put(It.IsAny<string>(), productDto))
            .ReturnsAsync(updated);

        // Act
        var result = await service.UpdateAsync(id, productDto);

        // Assert
        Assert.Equal(updated, result);
        _mockBaseHttpClient.Verify(x => x.Put(It.IsAny<string>(), productDto), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_IfInsertFail()
    {
        // Arrange
        var service = GetService();
        var input = GetProductDto();

        // Act
        await Assert.ThrowsAsync<ProductException>(async () => await service.AddAsync(input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Post<ProductDto>(It.IsAny<string>(), input), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnObject_IfInsertOk()
    {
        // Arrange
        var service = GetService();
        var input = GetProductDto();

        _mockBaseHttpClient.Setup(x => x.Post<ProductDto>(It.IsAny<string>(), input))
            .ReturnsAsync(input);

        // Act
        var result = await service.AddAsync(input);

        // Assert
        Assert.Equal(input, result);
        _mockBaseHttpClient.Verify(x => x.Post<ProductDto>(It.IsAny<string>(), input), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteAsync_ShouldReturnBool(bool deleted)
    {
        // Arrange
        var service = GetService();
        var input = _faker.Int(1);

        _mockBaseHttpClient.Setup(x => x.Delete(It.IsAny<string>()))
            .ReturnsAsync(deleted);

        // Act
        var result = await service.DeleteAsync(input);

        // Assert
        Assert.Equal(result, deleted);
        _mockBaseHttpClient.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
    }

    private IProductService GetService()
    {
        return new ProductService(_mockAppSettings.Object, _mockBaseHttpClient.Object);
    }

    private ProductDto GetProductDto()
    {
        return new ProductDto
        {
            Id = _faker.Int(1),
            CategoryId = _faker.Int(1),
            Name = _faker.String2(15, 30),
            Description = _faker.String2(15, 30)
        };
    }
}