using DMX.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DMX.Data
{
    public class DBInitializer
    {
        public readonly XContext dcx;

        public DBInitializer(XContext dContext)

        {
            dcx = dContext;

        }
       
        public async Task RoleCreation(IServiceProvider serviceProvider)
        {
            var rol = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            if (rol.Roles.Any() == false)
            {
                await rol.CreateAsync(new AppRole() { Name = "Basic" ,Rolename="Basic"});
                await rol.CreateAsync(new AppRole() { Name = "Manager",Rolename="Manager" });
                await rol.CreateAsync(new AppRole() { Name = "SuperAdmin", Rolename = "SuperAdmin" });
                await rol.CreateAsync(new AppRole() { Name = "Admin" , Rolename = "Admin"   });
            };




        }


        private readonly List<Claim> claimlist = new List<Claim>()
            {

                new Claim(ClaimTypes.Name,"SuperAdmin"),
                new Claim(ClaimTypes.Email,"oseipoku@gmail.com"),
                new Claim(ClaimTypes.Role,"SuperAdmin"),
                

            };
        IdentityResult identityResult;
        public async Task UserCreation(IServiceProvider serviceProvider)
        {
            var usm = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            if (await usm.FindByNameAsync("SuperAdmin") == null)
            {
                AppUser superUser = new AppUser()
                {
                    UserName = "SuperAdmin",
                    Surname = "SuperAdmin",
                    Firstname = "SuperAdmin",
                    Email = "admin@gmail.com",
                    PhoneNumber = "0244139692",
                    EmailConfirmed = true,

                };

                identityResult = await usm.CreateAsync(superUser, "OSP@SuperAdmin12345");
                if (identityResult.Succeeded)
                {
                    await usm.AddToRoleAsync(superUser, "SuperAdmin");
                    await usm.AddClaimsAsync(superUser, claimlist);
                };

            }


        }
    }
};


