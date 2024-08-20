using System.Diagnostics.CodeAnalysis;
using PhotosiApi.Dto.User;

namespace PhotosiApi.Service.User.Login;

[ExcludeFromCodeCoverage]
public class UserLoginHandler : IUserLoginHandler
{
    public List<LoggedUser> Users { get; set; } = [];
}