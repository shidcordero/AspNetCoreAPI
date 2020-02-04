using Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Data.Configurations
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            context.Database.EnsureCreated();
            if (!context.Users.Any())
            {
                AppUser user = new AppUser()
                {
                    Email = "superuser@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "superuser"
                };
                userManager.CreateAsync(user, "Superuser@12345!");
                context.SaveChanges();
            }
        }
    }
}
