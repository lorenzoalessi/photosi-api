using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotosiApi.Controllers;
using PhotosiApi.Service.User.Login;
using PhotosiApi.Utility;

namespace PhotosiApi.Security;

[ExcludeFromCodeCoverage]
public class ValidTokenAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var loginHandler = context.HttpContext.RequestServices.GetService<IUserLoginHandler>() ?? throw new NullReferenceException();
        
        var auth = context.HttpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(auth) || !auth.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var stringToken = auth.Split(" ")[1];

        // Controllo se c'è un utente loggato con lo stesso token
        var user = loginHandler.Users.SingleOrDefault(x => x.Token == stringToken);
        if (user == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        // Validazione del token
        var token = JwtUtils.DecodeToken(stringToken);
        if (!JwtUtils.IsValidToken(token, user.User))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (context.Controller is BaseController controller)
        {
            controller.LoggedUser = user;
        }
        else
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        base.OnActionExecuting(context);
    }
}