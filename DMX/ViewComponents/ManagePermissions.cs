using Microsoft.AspNetCore.Mvc;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using DMX.Models;
using DMX.DataProtection;
using System.Linq;
using System.Security.Claims;
namespace DMX.ViewComponents
{
    public class ManagePermissions(UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;


        public async Task<IViewComponentResult> InvokeAsync(string Id)
        {
            var model = new PermissionsVM();

            var userId = await usm.FindByIdAsync(@Encryption.Decrypt(Id));

            var userClaims = usm.GetClaimsAsync(userId).Result.Select(x => new ManagePermissionsVM
            {
            
            }).ToList();
        
    
        
   
            return View(userClaims);
        }
    }

}
