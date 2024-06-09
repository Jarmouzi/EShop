using EShop.Model.TypeSafe;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace EShop.IdentityService.Infrastructure.Authorizaion
{
    public class AuthorizeApiAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var controllerName = context.HttpContext.GetRouteData().Values["controller"]?.ToString();
            var actionName = context.HttpContext.GetRouteData().Values["action"]?.ToString();

            var claims = context.HttpContext.User.Claims;

            if (claims.Any(t => t.Type == ClaimTypes.Role && t.Value == "Admin") ||
                claims.Any(t => t.Type == controllerName && t.Value.Contains(actionName + ",")))
            {
                return;
            }
            context.Result = new  UnauthorizedObjectResult("شما به این سرویس دسترسی ندارید.");
        }
    }
    public class AuthorizePageAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var page = context.HttpContext.GetRouteData().Values["Page"]?.ToString();

            var pageParts = page?.Split('/');

            var claims = context.HttpContext.User.Claims;

            var request = context.HttpContext.Request.QueryString.Value;
            //if (!string.IsNullOrEmpty(request))
            //{
            //    var method = request.Split("handler=")[1].Split('&')[0];
            //    //.Substring()
            //}

            if (claims.Any(t => t.Type == ClaimTypes.Role && t.Value == "Admin") ||
                claims.Any(t => t.Type == pageParts?[1] && t.Value.Contains(pageParts?[2] + ",")))
            {
                return;
            }
            context.Result = new UnauthorizedObjectResult("شما به این بخش دسترسی ندارید!");
        }
    }
}
