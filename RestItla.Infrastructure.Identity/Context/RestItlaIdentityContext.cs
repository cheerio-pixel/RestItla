using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using RestItla.Infrastructure.Identity.Entities;

namespace RestItla.Infrastructure.Identity.Context
{
    public class RestItlaIdentityContext
    : IdentityDbContext<ApplicationUser>
    {
        public RestItlaIdentityContext(DbContextOptions<RestItlaIdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(entity => entity.ToTable(name: "Users"))
                   .Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"))
                   .Entity<IdentityUserRole<string>>(entity => entity.ToTable(name: "UserRoles"))
                   .Entity<IdentityUserLogin<string>>(entity => entity.ToTable(name: "UserLogins"));

        }
    }

}