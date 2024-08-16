using PhotosiApi.Dto;

namespace PhotosiApi.Service.User;

public interface IUserLoginHandler
{
    List<LoggedUser> Users { get; set; }
    
    LoggedUser CurrentUser { get; set; }
}