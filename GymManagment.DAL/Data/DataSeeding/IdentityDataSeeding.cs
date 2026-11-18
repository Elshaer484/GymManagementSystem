using GymManagment.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.DataSeeding
{
    public class IdentityDataSeeding 
    {
        public static async Task IdentityRoleAsync(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager , ILogger logger , CancellationToken ct = default)
        {
            try
            {
                bool hasroles = await roleManager.Roles.AnyAsync(ct);
                bool hasusers = await userManager.Users.AnyAsync(ct);

                if (hasroles && hasusers) return;

                var roles = new List<IdentityRole>()
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Admin")
            };

                foreach (var item in roles)
                {
                    if (!await roleManager.RoleExistsAsync(item.Name!))
                    {
                        var result = await roleManager.CreateAsync(item);
                        if (!result.Succeeded)
                        {
                            logger.LogError($"Failed To Create Role {item.Name} : {result.Errors.Select(e => e.Description)} ");
                        }
                    }
                }

                if (!hasusers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Youssef",
                        LastName = "Elsaher",
                        PhoneNumber = "01098167242",
                        Email = "elshaer.youssif@gmail.com",
                        UserName = "YoussefAmir"
                    };

                    await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Mohamed",
                        LastName = "Elsaher",
                        PhoneNumber = "01097167242",
                        Email = "elshaer.mohamed@gmail.com",
                        UserName = "MohamedAmir"
                    };

                    await userManager.CreateAsync(Admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(Admin, "Admin");
                    logger.LogInformation("Identity Data Sedded");
                    return;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Identity Seeding Failed");
                throw;
            }
        }
    }
}
