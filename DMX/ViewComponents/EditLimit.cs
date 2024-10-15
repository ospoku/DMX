using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    
    public class EditLimit(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        
        public IViewComponentResult Invoke(string Id)
        {

              
                PettyCashLimit limitToEdit = new();
                limitToEdit = (from p in dcx.PettyCashLimits where p.PettyCashLimitId == @Encryption.Decrypt(Id) select p).FirstOrDefault();

                EditLimitVM editLimitVM = new()
                {

              
                
                    Amount = (from x in dcx.PettyCashLimits where x.PettyCashLimitId == @Encryption.Decrypt(Id) select x.PettyCashLimitAmount).FirstOrDefault(),
                   
                };


                return View(editLimitVM);
            }
          

        }
    }


