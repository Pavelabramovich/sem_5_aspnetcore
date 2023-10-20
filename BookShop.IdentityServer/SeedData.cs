using BookShop.IdentityServer.Data;
using BookShop.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace BookShop.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                string adminRoleName = "admin";

                var adminRole = roleMgr.Roles.FirstOrDefault(role => role.Name == adminRoleName);

                if (adminRole is null)
                {
                    adminRole = new IdentityRole(adminRoleName);
                    roleMgr.CreateAsync(adminRole);
                }


                string userRoleName = "user";

                var userRole = roleMgr.Roles.FirstOrDefault(role => role.Name == userRoleName);

                if (userRole is null)
                {
                    userRole = new IdentityRole(userRoleName);
                    roleMgr.CreateAsync(userRole);
                }

                var admin = userMgr.FindByNameAsync("SuperAdmin").Result;
                if (admin is null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "SuperAdmin",
                        Email = "admin@email.com",
                        EmailConfirmed = true,
                    };

                    var result = userMgr.CreateAsync(admin, "Pass123$").Result;

                    if (result.Succeeded)
                        throw new Exception(result.Errors.First().Description);


                    result = userMgr.AddClaimsAsync(admin, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Admin Super"),
                            new Claim(JwtClaimTypes.GivenName, "Admin"),
                            new Claim(JwtClaimTypes.FamilyName, "Super"),
                            new Claim(JwtClaimTypes.WebSite, "http://super_admin.com"),
                        }).Result;

                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);

                    userMgr.AddToRoleAsync(admin, adminRoleName);

                    Log.Debug("admin created");
                }
                else
                {
                    userMgr.AddToRoleAsync(admin, adminRoleName);
                    Log.Debug("admin already exists");
                }

                var user = userMgr.FindByNameAsync("User").Result;
                if (user is null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "User",
                        Email = "user@email.com",
                        EmailConfirmed = true
                    };

                    var result = userMgr.CreateAsync(user, "Pass123$").Result;

                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);

                    result = userMgr.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "User Smith"),
                            new Claim(JwtClaimTypes.GivenName, "User"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://user.com"),
                            new Claim("location", "somewhere")
                        }).Result;

                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);

                    userMgr.AddToRoleAsync(user, userRoleName);

                    Log.Debug("user created");
                }
                else
                {
                    Log.Debug("user already exists");
                }
            }
        }
    }
}