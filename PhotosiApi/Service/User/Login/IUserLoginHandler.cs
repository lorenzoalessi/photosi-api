﻿using PhotosiApi.Dto.User;

namespace PhotosiApi.Service.User.Login;

public interface IUserLoginHandler
{
    List<LoggedUser> Users { get; set; }
}