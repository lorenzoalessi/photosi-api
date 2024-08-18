using PhotosiApi.Dto;

namespace PhotosiApi.Service.User.Login;

public class UserLoginHandler : IUserLoginHandler
{
    public List<LoggedUser> Users { get; set; } = [];
}