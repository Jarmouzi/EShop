using EShop.Model.TypeSafe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace EShop.IdentityService.Infrastructure.Authorizaion
{
    public class AuthorizeApiAttribute : Attribute, IAsyncAuthorizationFilter
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
    public class AuthorizePageAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var page = context.HttpContext.GetRouteData().Values["Page"]?.ToString();

            var pageParts = page?.Split('/');

            var claims = context.HttpContext.User.Claims;
            var roles = context.HttpContext.User.IsInRole("admin");

            var request = context.HttpContext.Request.QueryString.Value;
            if (!string.IsNullOrEmpty(request))
            {
                var method = request.Split("handler=")[1].Split('&')[0];
                //.Substring()
            }

            if (context.HttpContext.User.IsInRole("admin") ||
                claims.Any(t => t.Type == pageParts?[1] && t.Value.Contains(pageParts?[2] + ",")))
            {
                return;
            }
            context.Result = new UnauthorizedObjectResult("You dont have access to this functionality.");
        }
    }
}
