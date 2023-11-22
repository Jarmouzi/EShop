using EShop.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.DataContext
{
    public class UserIdentityContext : IdentityDbContext
    {
        public UserIdentityContext(DbContextOptions<UserIdentityContext> options) : base(options)
        {
        }
    }
}
