using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagementService.Models.Authentication.SignUp;

namespace UserManagementService.Models
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public DbSet<PreRegisteredUser> PreRegisteredUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name="Admin", ConcurrencyStamp="1", NormalizedName="Admin"},
                new IdentityRole() { Name = "Doctor", ConcurrencyStamp = "2", NormalizedName = "Doctor" },
                new IdentityRole() { Name = "Patient", ConcurrencyStamp = "3", NormalizedName = "Patient" }

                );
        }
        
    }
}
