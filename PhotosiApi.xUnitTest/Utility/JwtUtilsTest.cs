using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PhotosiApi.Dto.User;
using PhotosiApi.Utility;

namespace PhotosiApi.xUnitTest.Utility;

public class JwtUtilsTest : TestSetup
{
    [Fact]
    public void GenerateJwtToken_ShouldReturnToken_Always()
    {
        // Arrange
        var userDto = GetUserDto();

        // Act
        var token = JwtUtils.GenerateJwtToken(userDto);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void DecodeToken_ShouldReturnUserDto_Always()
    {
        // Arrange
        var userDto = GetUserDto();
        var token = JwtUtils.GenerateJwtToken(userDto);

        // Act
        var result = JwtUtils.DecodeToken(token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("photosi", result.Issuer);
    }

    [Fact]
    public void IsValidToken_ShouldReturnTrue_WhenValidToken()
    {
        // Arrange
        var userDto = GetUserDto();
        var token = GenerateJwtToken(userDto);
        
        // Act
        var result = JwtUtils.IsValidToken(token, userDto);
        
        // Assert
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void IsValidToken_ShouldReturnFalse_WhenIssuerIsInvalid()
    {
        // Arrange
        var userDto = GetUserDto();
        var token = GenerateJwtToken(userDto, issuer: "invalid-issuer");
    
        // Act
        var result = JwtUtils.IsValidToken(token, userDto);
    
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidToken_ShouldReturnFalse_WhenAudienceIsInvalid()
    {
        // Arrange
        var userDto = GetUserDto();
        var token = GenerateJwtToken(userDto, audiance: "invalid-audience");
    
        // Act
        var result = JwtUtils.IsValidToken(token, userDto);
    
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidToken_ShouldReturnFalse_WhenClaimsAreInvalid()
    {
        // Arrange
        var userDto = GetUserDto();
        var fakeClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _faker.Int(1).ToString())
        };
        var token = GenerateJwtToken(userDto, fakeClaims: fakeClaims);
    
        // Act
        var result = JwtUtils.IsValidToken(token, userDto);
    
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsValidToken_ShouldReturnFalse_WhenTokenIsExpired()
    {
        // Arrange
        var userDto = GetUserDto();
        var token = GenerateJwtToken(userDto, expiration: DateTime.Now.AddDays(-1));
        
        // Act
        var result = JwtUtils.IsValidToken(token, userDto);
    
        // Assert
        Assert.False(result);
    }
    
    private UserDto GetUserDto()
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
    
    private JwtSecurityToken GenerateJwtToken(UserDto userDto, string? issuer = null, string? audiance = null, IEnumerable<Claim> fakeClaims = null, DateTime? expiration = null)
    {
        // Claims
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };

        // Generazione token
        return new JwtSecurityToken(
            issuer: issuer ?? "photosi",
            audience: audiance ?? userDto.Username,
            claims: fakeClaims ?? claims,
            expires: expiration ?? DateTime.Now.AddDays(1),
            signingCredentials: null // Tanto non faccio nessun controllo
        );
    }
}