using DMX.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DMX.Data
{
    public  class DBInitializer(XContext dContext, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        public readonly XContext dcx = dContext;
        public readonly RoleManager<AppRole> rol = roleManager;
        public readonly UserManager<AppUser> usm = userManager;
        public async Task  Initialize()
        {
            dcx.Database.EnsureCreated();
            if (!rol.Roles.Any())
            {
                await rol.CreateAsync(new AppRole() { Name = "Basic", Rolename = "Basic", Description = "Role for basic users" });
                await rol.CreateAsync(new AppRole() { Name = "Manager", Rolename = "Manager", Description = "Role for managers" });
                await rol.CreateAsync(new AppRole() { Name = "SuperAdmin", Rolename = "SuperAdmin", Description = "Role for superadmin" });
                await rol.CreateAsync(new AppRole() { Name = "Admin", Rolename = "Admin", Description = "Role for admin users" });
            }

            if (!dcx.DeceasedTypes.Any()) {
                dcx.DeceasedTypes.Add(new DeceasedType()
                {
                    Name = "Brought In Dead",
                    Code = "BID",
                    Description = "Name for a Patient who was broguth in Dead"
                });
                dcx.DeceasedTypes.Add(new DeceasedType()
                {
                    Name = "Dead In Ward",
                    Code = "DIW",
                    Description = "Name for a Patient who Died in Ward"
                });
            }

            List<Claim> claimlist =
            [

                new(ClaimTypes.Name,"SuperAdmin"),
                new Claim(ClaimTypes.Email,"oseipoku@gmail.com"),
                new Claim(ClaimTypes.Role,"SuperAdmin"),


            ];

            List<Claim> claimlist2 =
                 [

                     new Claim(ClaimTypes.Name,"Admin"),
                new Claim(ClaimTypes.Email,"oseipoku2@gmail.com"),
                new Claim(ClaimTypes.Role,"Admin"),


            ];
            IdentityResult identityResult;


            if (await usm.FindByNameAsync("SuperAdmin") == null)
            {
                AppUser superUser = new()
                {
                    UserName = "SuperAdmin",
                    Surname = "SuperAdmin",
                    Firstname = "SuperAdmin",
                    Email = "superadmin@gmail.com",
                    PhoneNumber = "0244139692",
                    EmailConfirmed = true,
                    DepartmentId = "xxxxxxxxxxxxxxxxx",
                    RankId = "xxxxxxxxxxxxxxxxxx",
                };

                identityResult = await usm.CreateAsync(superUser, "OSP@SuperAdmin12345");
                if (identityResult.Succeeded)
                {
                    await usm.AddToRoleAsync(superUser, "SuperAdmin");
                    await usm.AddClaimsAsync(superUser, claimlist);
                };

            };
            if (await usm.FindByNameAsync("Admin") == null)
            {
                AppUser superUser = new()
                {
                    UserName = "Admin",
                    Surname = "Admin",
                    Firstname = "Admin",
                    Email = "admin@gmail.com",
                    PhoneNumber = "0244139692",
                    EmailConfirmed = true,
                    DepartmentId = "xxxxxxxxxxxxxxxxx",
                    RankId = "xxxxxxxxxxxxxxxxxx",
                };

                identityResult = await usm.CreateAsync(superUser, "OSP@Admin12345");
                if (identityResult.Succeeded)
                {
                    await usm.AddToRoleAsync(superUser, "Admin");
                    await usm.AddClaimsAsync(superUser, claimlist2);
                };

            };
          
        }
    }
}



  


