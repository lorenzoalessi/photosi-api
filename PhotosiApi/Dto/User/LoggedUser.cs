using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Dto.User;

[ExcludeFromCodeCoverage]
public class LoggedUser
{
    public UserDto User { get; set; }
    
    public string Token { get; set; }
}