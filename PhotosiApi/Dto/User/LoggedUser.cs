namespace PhotosiApi.Dto;

public class LoggedUser
{
    public UserDto User { get; set; }
    
    public string Token { get; set; }
}