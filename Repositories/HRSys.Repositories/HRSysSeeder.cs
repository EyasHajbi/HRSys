using HRSys.Enum;
using HRSys.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Repositories
{
    public static class SeedData
    {
        public static async Task SeedAsync(HRSysContext dbContext)
        {
            var user = new ApplicationUser
            {
                UserName = "Admin",
                NormalizedUserName = "Admin",
                Email = "ahasan@link.sa‎",
                NormalizedEmail = "ahasan@link.sa",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

           // var roleStore = new RoleStore<IdentityRole>(dbContext);
           
            //if (!dbContext.Roles.Any(r => r.Name == "admin"))
            //{
            //    await roleStore.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "admin" });
            //}
            var adminUser = dbContext.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (adminUser == null)
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "123456");
                user.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(dbContext);
                await userStore.CreateAsync(user);
                //await userStore.AddToRoleAsync(user, "admin");
            }
           

            user = new ApplicationUser
            {
                UserName = "User1",
                NormalizedUserName = "User1",
                Email = "ahasan@link.sa‎",
                NormalizedEmail = "ahasan@link.sa",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (!dbContext.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "123456");
                user.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(dbContext);
                await userStore.CreateAsync(user);
               // await userStore.AddToRoleAsync(user, "admin");
            }

            user = new ApplicationUser
            {
                UserName = "User2",
                NormalizedUserName = "User2",
                Email = "ahasan@link.sa‎",
                NormalizedEmail = "ahasan@link.sa",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (!dbContext.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "123456");
                user.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(dbContext);
                await userStore.CreateAsync(user);
                //await userStore.AddToRoleAsync(user, "admin");
            }
   
        }
    }
  public static class HRSysSeeder
    {
        public static void Seed(this IHost webhost)
        {
            using (var scope = webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                // alternatively resolve UserManager instead and pass that if only think you want to seed are the users     
                using (var dbContext = scope.ServiceProvider.GetRequiredService<HRSysContext>())
                {
                   SeedData.SeedAsync(dbContext).GetAwaiter().GetResult();
                }
            }
        }
    }
   
}
