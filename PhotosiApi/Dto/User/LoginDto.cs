using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Dto.User;

[ExcludeFromCodeCoverage]
public class LoginDto
{
    public string Username { get; set; }
    
    public string Password { get; set; }
}