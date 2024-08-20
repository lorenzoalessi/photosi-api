using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using PhotosiApi.Dto.User;

namespace PhotosiApi.Controllers;

[ExcludeFromCodeCoverage]
public class BaseController : ControllerBase
{
    public LoggedUser? LoggedUser { get; set; }
}