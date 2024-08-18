using Moq;
using PhotosiApi.Dto.AddressBook;
using PhotosiApi.Dto.Order;
using PhotosiApi.Dto.Product;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients;
using PhotosiApi.Service.Order;

namespace PhotosiApi.xUnitTest.Service;

public class OrderServiceTest : TestSetup
{
    private readonly Mock<IBaseHttpClient> _mockBaseHttpClient;

    public OrderServiceTest()
    {
        _mockBaseHttpClient = new Mock<IBaseHttpClient>();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task DeleteAsync_ShouldReturnBool_Always(bool deleted)
    {
        // Arrange
        var service = GetOrderService();
        var input = _faker.Int(1);

        _mockBaseHttpClient.Setup(x => x.Delete(It.IsAny<string>()))
            .ReturnsAsync(deleted);

        // Act
        var result = await service.DeleteAsync(input);

        // Assert
        Assert.Equal(deleted, result);
        _mockBaseHttpClient.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_IfProductNotExist()
    {
        // Arrange
        var service = GetOrderService();
        var input = GenerateOrderDto();

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.AddAsync(input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<ProductDto>(It.IsAny<string>()),
            Times.Exactly(input.OrderProducts.Count));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Never);
        _mockBaseHttpClient.Verify(x => x.Post<OrderDto>(It.IsAny<string>(), input), Times.Never);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_IfAddressNotExist()
    {
        // Arrange
        var service = GetOrderService();
        var input = GenerateOrderDto();

        _mockBaseHttpClient.Setup(x => x.Get<ProductDto>(It.IsAny<string>()))
            .ReturnsAsync(new ProductDto());

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.AddAsync(input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<ProductDto>(It.IsAny<string>()),
            Times.Exactly(input.OrderProducts.Count));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Post<OrderDto>(It.IsAny<string>(), input), Times.Never);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_IfOrderHttpError()
    {
        // Arrange
        var service = GetOrderService();
        var input = GenerateOrderDto();

        _mockBaseHttpClient.Setup(x => x.Get<ProductDto>(It.IsAny<string>()))
            .ReturnsAsync(new ProductDto());

        _mockBaseHttpClient.Setup(x => x.Get<AddressBookDto>(It.IsAny<string>()))
            .ReturnsAsync(new AddressBookDto());

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.AddAsync(input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<ProductDto>(It.IsAny<string>()),
            Times.Exactly(input.OrderProducts.Count));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Post<OrderDto>(It.IsAny<string>(), input), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnObject_IfOrderHttpOk()
    {
        // Arrange
        var service = GetOrderService();
        var input = GenerateOrderDto();

        _mockBaseHttpClient.Setup(x => x.Get<ProductDto>(It.IsAny<string>()))
            .ReturnsAsync(new ProductDto());

        _mockBaseHttpClient.Setup(x => x.Get<AddressBookDto>(It.IsAny<string>()))
            .ReturnsAsync(new AddressBookDto());

        _mockBaseHttpClient.Setup(x => x.Post<OrderDto>(It.IsAny<string>(), input))
            .ReturnsAsync(input);

        // Act
        var result = await service.AddAsync(input);

        // Assert
        Assert.Equal(input, result);
        _mockBaseHttpClient.Verify(x => x.Get<ProductDto>(It.IsAny<string>()),
            Times.Exactly(input.OrderProducts.Count));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Post<OrderDto>(It.IsAny<string>(), input), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_IfOrderNotFound()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);
        var input = GenerateOrderDto(id);

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.UpdateAsync(id, input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<OrderDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<ProductDto>(It.IsAny<string>()), Times.Never);
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Never);
        _mockBaseHttpClient.Verify(x => x.Post<OrderDto>(It.IsAny<string>(), input), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_IfProductsNotFound()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);
        var input = GenerateOrderDto(id);

        _mockBaseHttpClient.Setup(x => x.Get<OrderDto>(It.IsAny<string>()))
            .ReturnsAsync(new OrderDto());

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.UpdateAsync(id, input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<OrderDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<ProductDto>(It.IsAny<string>()),
            Times.Exactly(input.OrderProducts.Count));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Never);
        _mockBaseHttpClient.Verify(x => x.Post<OrderDto>(It.IsAny<string>(), input), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_IfAddressNotFound()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);
        var input = GenerateOrderDto(id);

        _mockBaseHttpClient.Setup(x => x.Get<OrderDto>(It.IsAny<string>()))
            .ReturnsAsync(new OrderDto());

        _mockBaseHttpClient.Setup(x => x.Get<ProductDto>(It.IsAny<string>()))
            .ReturnsAsync(new ProductDto());

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.UpdateAsync(id, input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<OrderDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<ProductDto>(It.IsAny<string>()),
            Times.Exactly(input.OrderProducts.Count));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Post<OrderDto>(It.IsAny<string>(), input), Times.Never);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task UpdateAsync_ShouldReturnBool_IfAddressOk(bool putOk)
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);
        var input = GenerateOrderDto(id);

        _mockBaseHttpClient.Setup(x => x.Get<OrderDto>(It.IsAny<string>()))
            .ReturnsAsync(new OrderDto());

        _mockBaseHttpClient.Setup(x => x.Get<ProductDto>(It.IsAny<string>()))
            .ReturnsAsync(new ProductDto());

        _mockBaseHttpClient.Setup(x => x.Get<AddressBookDto>(It.IsAny<string>()))
            .ReturnsAsync(new AddressBookDto());

        _mockBaseHttpClient.Setup(x => x.Put(It.IsAny<string>(), input))
            .ReturnsAsync(putOk);

        // Act
        var result = await service.UpdateAsync(id, input);

        // Assert
        Assert.Equal(putOk, result);
        _mockBaseHttpClient.Verify(x => x.Get<OrderDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<ProductDto>(It.IsAny<string>()),
            Times.Exactly(input.OrderProducts.Count));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Put(It.IsAny<string>(), input), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_IfOrdersNotFound()
    {
        // Arrange
        var service = GetOrderService();
        var input = _faker.Int(1);

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.GetByIdAsync(input));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<OrderDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<EntireProductDto>(It.IsAny<string>()), Times.Never);
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_IfProductsNotFound()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);
        var input = GenerateOrderDto();

        _mockBaseHttpClient.Setup(x => x.Get<OrderDto>(It.IsAny<string>()))
            .ReturnsAsync(input);

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.GetByIdAsync(id));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<OrderDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<EntireProductDto>(It.IsAny<string>()),
            Times.Exactly(input.OrderProducts.Count));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_IfAddressBookNotFound()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);
        var input = GenerateOrderDto();

        _mockBaseHttpClient.Setup(x => x.Get<OrderDto>(It.IsAny<string>()))
            .ReturnsAsync(input);

        _mockBaseHttpClient.Setup(x => x.Get<EntireProductDto>(It.IsAny<string>()))
            .ReturnsAsync(new EntireProductDto
            {
                Quantity = _faker.Int(1, 10)
            });

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.GetByIdAsync(id));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<OrderDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<EntireProductDto>(It.IsAny<string>()),
            Times.Exactly(input.OrderProducts.Count));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOk_IfAddressBookFound()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);
        var input = GenerateOrderDto();

        _mockBaseHttpClient.Setup(x => x.Get<OrderDto>(It.IsAny<string>()))
            .ReturnsAsync(input);

        var entireProductDto = new EntireProductDto
        {
            Quantity = _faker.Int(1, 10)
        };
        _mockBaseHttpClient.Setup(x => x.Get<EntireProductDto>(It.IsAny<string>()))
            .ReturnsAsync(entireProductDto);

        var addressBookDto = new AddressBookDto
        {
            Id = _faker.Int(1),
            AddressName = _faker.String2(15, 30),
            AddressNumber = _faker.String2(15, 30),
            Cap = _faker.String2(15, 30),
            CityName = _faker.String2(15, 30),
            CountryName = _faker.String2(15, 30)
        };
        _mockBaseHttpClient.Setup(x => x.Get<AddressBookDto>(It.IsAny<string>()))
            .ReturnsAsync(addressBookDto);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<OrderDto>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<EntireProductDto>(It.IsAny<string>()),
            Times.Exactly(input.OrderProducts.Count));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Once);
        Assert.Equal(input.Id, result.Id);
        Assert.Equal(input.OrderCode, result.OrderCode);
        Assert.Equal(input.UserId, result.UserId);
        Assert.Equal(addressBookDto, result.AddressBookDto);
    }

    [Fact]
    public async Task GetAllForUser_ShouldThrowException_IfOrdersNotFound()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.GetAllForUser(id));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<List<OrderDto>>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<EntireProductDto>(It.IsAny<string>()), Times.Never);
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetAllForUser_ShouldReturnEmptyList_IfOrdersEmpty()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);

        _mockBaseHttpClient.Setup(x => x.Get<List<OrderDto>>(It.IsAny<string>()))
            .ReturnsAsync([]);

        // Act
        var result = await service.GetAllForUser(id);

        // Assert
        Assert.Empty(result);
        _mockBaseHttpClient.Verify(x => x.Get<List<OrderDto>>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<EntireProductDto>(It.IsAny<string>()), Times.Never);
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetAllForUser_ShouldThrowException_IfOrdersProductNotFound()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);

        var ordersDto = Enumerable.Range(0, _faker.Int(5, 10))
            .Select(x => GenerateOrderDto())
            .ToList();

        _mockBaseHttpClient.Setup(x => x.Get<List<OrderDto>>(It.IsAny<string>()))
            .ReturnsAsync(ordersDto);

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.GetAllForUser(id));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<List<OrderDto>>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<EntireProductDto>(It.IsAny<string>()),
            Times.Exactly(ordersDto.Sum(x => x.OrderProducts.Count)));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetAllForUser_ShouldThrowException_IfAddresBookNotFound()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);

        var ordersDto = Enumerable.Range(0, _faker.Int(5, 10))
            .Select(x => GenerateOrderDto())
            .ToList();

        _mockBaseHttpClient.Setup(x => x.Get<List<OrderDto>>(It.IsAny<string>()))
            .ReturnsAsync(ordersDto);

        _mockBaseHttpClient.Setup(x => x.Get<EntireProductDto>(It.IsAny<string>()))
            .ReturnsAsync(new EntireProductDto());

        // Act
        await Assert.ThrowsAsync<OrderException>(async () => await service.GetAllForUser(id));

        // Assert
        _mockBaseHttpClient.Verify(x => x.Get<List<OrderDto>>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<EntireProductDto>(It.IsAny<string>()),
            Times.Exactly(ordersDto.Sum(x => x.OrderProducts.Count)));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Exactly(ordersDto.Count));
    }

    [Fact]
    public async Task GetAllForUser_ShouldReturnList_IfAddressBookFound()
    {
        // Arrange
        var service = GetOrderService();
        var id = _faker.Int(1);

        var ordersDto = Enumerable.Range(0, _faker.Int(5, 10))
            .Select(x => GenerateOrderDto())
            .ToList();

        _mockBaseHttpClient.Setup(x => x.Get<List<OrderDto>>(It.IsAny<string>()))
            .ReturnsAsync(ordersDto);

        _mockBaseHttpClient.Setup(x => x.Get<EntireProductDto>(It.IsAny<string>()))
            .ReturnsAsync(new EntireProductDto());

        _mockBaseHttpClient.Setup(x => x.Get<AddressBookDto>(It.IsAny<string>()))
            .ReturnsAsync(new AddressBookDto());

        // Act
        var result = await service.GetAllForUser(id);

        // Assert
        Assert.Equal(ordersDto.Count, result.Count);
        _mockBaseHttpClient.Verify(x => x.Get<List<OrderDto>>(It.IsAny<string>()), Times.Once);
        _mockBaseHttpClient.Verify(x => x.Get<EntireProductDto>(It.IsAny<string>()),
            Times.Exactly(ordersDto.Sum(x => x.OrderProducts.Count)));
        _mockBaseHttpClient.Verify(x => x.Get<AddressBookDto>(It.IsAny<string>()), Times.Exactly(ordersDto.Count));
    }

    private IOrderService GetOrderService()
    {
        return new OrderService(_mockAppSettings.Object, _mockBaseHttpClient.Object);
    }

    private OrderDto GenerateOrderDto(int? id = null)
    {
        return new OrderDto
        {
            Id = id ?? _faker.Int(1),
            OrderCode = _faker.Int(1),
            UserId = _faker.Int(1),
            AddressId = _faker.Int(1),
            OrderProducts = Enumerable.Range(0, _faker.Int(10, 30))
                .Select(_ => GenerateOrderProducts())
                .ToList()
        };
    }

    private OrderProductDto GenerateOrderProducts()
    {
        return new OrderProductDto
        {
            Quantity = _faker.Int(1)
        };
    }
}