using EShop.Model;
using System.Security.Claims;
using System.Security.Principal;

namespace EShop.IdentityService.Identity
{
    public interface IAuthService
    {
        Task<Guid?> Login(LoginUser credentials);
        Task Logout();
        Task<bool> RegisterUser(LoginUser user);
        Task<bool> AddUserClaim(string user, Claim claim);
        Task<bool> AddUpdateClaim(string user, Claim claim);
        Task GenerateCookieAuthentication(string username);
        Task<string> GenerateTokenString(string user, JwtConfiguration jwtConfig);
    }
}