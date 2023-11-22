using EShop.Model.TypeSafe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace EShop.IdentityService.Infrastructure.Authorizaion
{
    public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var controllerName = context.HttpContext.GetRouteData().Values["controller"]?.ToString();
            var actionName = context.HttpContext.GetRouteData().Values["action"]?.ToString();

            var claims = context.HttpContext.User.Claims;

            if (claims.Any(t => t.Type == controllerName && t.Value.Contains(actionName + ","))
               // || IS ADMIN
                )
            {
                return;
            }
            context.Result = new UnauthorizedObjectResult("You dont have access to this functionality.");
        }
    }
}
