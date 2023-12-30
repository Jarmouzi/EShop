using EShop.Services.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services.DataContexts
{
    public class UserIdentityContext : IdentityDbContext
    {
        public UserIdentityContext(DbContextOptions<UserIdentityContext> options) : base(options)
        {
        }
    }
}
