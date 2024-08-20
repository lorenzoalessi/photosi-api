using System.Net;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using PhotosiApi.Dto.User;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients;

namespace PhotosiApi.xUnitTest.HttpClients;

public class BaseHttpClientTest : TestSetup
{
    private readonly Mock<HttpMessageHandler> _handler;
    private const string URL = "https://example.com/api/stuff";
    
    public BaseHttpClientTest()
    {
        _handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
    }

    [Fact]
    public async Task Get_ShouldThrowException_IfNotSuccessStatusCode()
    {
        // Arrange
        _handler.SetupRequest(HttpMethod.Get, URL)
            .ReturnsResponse(HttpStatusCode.InternalServerError, string.Empty);
        
        var client = _handler.CreateClient();
        var baseHttpClient = GetHttpClient(client);

        // Act
        await Assert.ThrowsAsync<BaseHttpClientException>(async () => await baseHttpClient.Get<UserDto>(URL));

        // Assert
    }
    
    [Fact]
    public async Task Get_ShouldReturnDefault_IfStatusCodeNotOk()
    {
        // Arrange
        _handler.SetupRequest(HttpMethod.Get, URL)
            .ReturnsResponse(HttpStatusCode.NoContent, string.Empty);
        
        var client = _handler.CreateClient();
        var baseHttpClient = GetHttpClient(client);

        // Act
        var result = await baseHttpClient.Get<UserDto>(URL);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task Get_ShouldReturnJson_IfStatusCodeOk()
    {
        // Arrange
        var obj = GenerateObject();
        var jsonObj = JsonConvert.SerializeObject(obj);

        _handler.SetupRequest(HttpMethod.Get, URL).ReturnsResponse(HttpStatusCode.OK, jsonObj);
        var client = _handler.CreateClient();
        var baseHttpClient = GetHttpClient(client);

        // Act
        var result = await baseHttpClient.Get<UserDto>(URL);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(obj.Id, result.Id);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Put_ShouldReturnBool_Always(bool updated)
    {
        // Arrange
        var obj = GenerateObject();
        var jsonObj = JsonConvert.SerializeObject(obj);

        _handler.SetupRequest(HttpMethod.Put, URL).ReturnsResponse(updated ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, jsonObj);
        var client = _handler.CreateClient();
        var baseHttpClient = GetHttpClient(client);
        
        // Act
        var result = await baseHttpClient.Put(URL, obj);

        // Assert
        Assert.Equal(updated, result);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Delete_ShouldReturnBool_Always(bool updated)
    {
        // Arrange
        _handler.SetupRequest(HttpMethod.Delete, URL)
            .ReturnsResponse(updated ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
        var client = _handler.CreateClient();
        var baseHttpClient = GetHttpClient(client);
        
        // Act
        var result = await baseHttpClient.Delete(URL);

        // Assert
        Assert.Equal(updated, result);
    }

    [Fact]
    public async Task Post_ShouldThrowException_IfNotSuccessStatusCode()
    {
        // Arrange
        var obj = GenerateObject();
        
        _handler.SetupRequest(HttpMethod.Post, URL)
            .ReturnsResponse(HttpStatusCode.InternalServerError);
        var client = _handler.CreateClient();
        var baseHttpClient = GetHttpClient(client);
        
        // Act
        await Assert.ThrowsAsync<BaseHttpClientException>(async () => await baseHttpClient.Post<UserDto>(URL, obj));
    }
    
    [Fact]
    public async Task Post_ShouldReturnObject_IfSuccessStatusCode()
    {
        // Arrange
        var obj = GenerateObject();
        var jsonObj = JsonConvert.SerializeObject(obj);
        
        _handler.SetupRequest(HttpMethod.Post, URL)
            .ReturnsResponse(HttpStatusCode.OK, jsonObj);
        var client = _handler.CreateClient();
        var baseHttpClient = GetHttpClient(client);
        
        // Act
        var response = await baseHttpClient.Post<UserDto>(URL, obj);
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(obj.Id, response.Id);
    }
    
    private IBaseHttpClient GetHttpClient(HttpClient httpClient)
    {
        return new BaseHttpClient(httpClient);
    }

    private UserDto GenerateObject()
    {
        return new UserDto
        {
            Id = _faker.Int(1)
        };
    }
}