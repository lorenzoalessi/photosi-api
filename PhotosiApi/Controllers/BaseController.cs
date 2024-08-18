using Microsoft.AspNetCore.Mvc;
using PhotosiApi.Dto;

namespace PhotosiApi.Controllers;

public class BaseController : ControllerBase
{
    public LoggedUser? LoggedUser { get; set; }
}