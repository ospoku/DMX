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

              
                CashLimit limitToEdit = new();
                limitToEdit = (from p in dcx.CashLimits where p.CashLimitId == @Encryption.Decrypt(Id) select p).FirstOrDefault();

                EditLimitVM editLimitVM = new()
                {

              
                
                    Amount = (from x in dcx.CashLimits where x.CashLimitId == @Encryption.Decrypt(Id) select x.Amount).FirstOrDefault(),
                   
                };


                return View(editLimitVM);
            }
          

        }
    }


