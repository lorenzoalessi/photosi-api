using PhotosiApi.Dto;

namespace PhotosiApi.Service.User;

public class UserLoginHandler : IUserLoginHandler
{
    public List<LoggedUser> Users { get; set; } = [];

    public LoggedUser CurrentUser { get; set; }
}