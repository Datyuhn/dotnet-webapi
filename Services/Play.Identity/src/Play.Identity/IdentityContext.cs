using Microsoft.EntityFrameworkCore;
using Play.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Play.Identity
{
    public class IdentityContext : IdentityDbContext<AppUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
            
        }
    }
}