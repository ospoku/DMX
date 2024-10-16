using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DMX.Data;
using DMX.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using DMX.Models;
using Microsoft.AspNetCore.Identity;
using DMX.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using DMX.Helpers;
using DMX.Services;

namespace DMX.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SettingsController(XContext context, INotyfService notyfService, UserManager<AppUser> userManager, EntityService entityService ) : Controller
    {
        public readonly XContext dcx = context;
        public readonly INotyfService notyf = notyfService;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly EntityService entityServ = entityService;
    
        public IActionResult Preferences()
        {

            return ViewComponent("ViewPreferences");
        }
        public IActionResult SystemSetup()
        {
            return ViewComponent("SystemSetup", "ViewDepartments");
        }

     


            

            [HttpPost]
        public async Task<IActionResult> AddTravelTypeAsync(AddTravelTypeVM addTravelTypeVM)
            {
                string RefN = "T" + Guid.NewGuid().ToString().Substring(0, 5);

                TravelType travelType = new()
                {
                    Name = addTravelTypeVM.Name,
                    Code = addTravelTypeVM.Code,
                    Description = addTravelTypeVM.Description,
                };

                if (await entityServ.AddEntityAsync(travelType, User))
                {
                    return RedirectToAction("SystemSetup");
                }
                return RedirectToAction("SystemSetup");
            
        }
        [HttpPost]
        public async Task<IActionResult> AddFeeStructureAsync(AddFeeStructureVM addFeeStructureVM)
        {
          
            string RefN = "F" + Guid.NewGuid().ToString("N").Substring(0, 5);
            var user = await usm.GetUserAsync(User);

            FeeStructure addThisStructure = new()
            {
                Name = addFeeStructureVM.Name,

                MinDays = addFeeStructureVM.Min,
                MaxDays = addFeeStructureVM.Max,
                Fee = addFeeStructureVM.Fee,
                CreatedBy = user?.UserName,
                CreatedDate = DateTime.UtcNow,
            };


            if (await saveHelper.SaveEntity(addThisStructure, user.UserName))
            {


                return RedirectToAction("SystemSetup");
            }
                return RedirectToAction("SystemSetup");
            }
        
        [HttpPost]
        public async Task<IActionResult> AddDeceasedTypeAsync(AddDeceasedTypeVM addDeceasedTypeVM)
        {
         
            string RefN = "D" + Guid.NewGuid().ToString().Substring(0, 5);
            var user = await usm.GetUserAsync(User);

            DeceasedType addThisDeceasedType = new()
            {
                Name = addDeceasedTypeVM.Name,

                Code = addDeceasedTypeVM.Code,
                Description = addDeceasedTypeVM.Description,
                CreatedBy = usm.GetUserAsync(User).Result?.UserName,
                CreatedDate = DateTime.UtcNow,
            };


            if (await saveHelper.SaveEntity(addThisDeceasedType, user?.UserName))
            {
               

                return RedirectToAction("SystemSetup");
            }
          
        
                return RedirectToAction("SystemSetup");
            }
        
        [HttpPost]
        public async Task<IActionResult> AddDepartmentAsync(AddDepartmentVM addDepartmentVM)
        {
            var user = await usm.GetUserAsync(User);
         
            string RefN = "D" + Guid.NewGuid().ToString("N").Substring(0, 5);

            Department addThisDepartment = new()
            {
                Name = addDepartmentVM.Name,

                Code = addDepartmentVM.Code,
                Description = addDepartmentVM.Description,
                CreatedBy = user?.UserName,
                CreatedDate = DateTime.UtcNow,
            };
      

            if (await saveHelper.SaveEntity(addThisDepartment,user.UserName))
            {
          

                return RedirectToAction("SystemSetup");
            }
 
              
                return RedirectToAction("SystemSetup");
            }
        

        [HttpGet]
        public IActionResult AddPerDiem()
        {
            return ViewComponent("AddPerDiem");
        }

        [HttpPost]
        public async Task<IActionResult> SavePCLimit(EditLimitVM limitVM, CashLimit cashLimit)
        {
            var user = await usm.GetUserAsync(User);
            // Assuming you save the limit in a database or some other store

            CashLimit limitToUpdate = (from a in dcx.CashLimits where a.CashLimitId == cashLimit.CashLimitId select a).FirstOrDefault();


            limitToUpdate.Amount = cashLimit.Amount;
            limitToUpdate.CreatedBy = user?.UserName;
            limitToUpdate.CreatedDate = DateTime.UtcNow;
            dcx.CashLimits.Attach(limitToUpdate);
            dcx.Entry(limitToUpdate).State = EntityState.Modified;

            if (await saveHelper.SaveEntity(limitToUpdate,user?.UserName))
            {
                notyf.Success("Record successfully saved", 5);

                return RedirectToAction("SystemSetup");
            }
            else
            {
                notyf.Error("Error, Record could not be saved!!!", 5);
                return RedirectToAction("SystemSetup");
            }


        }
        [HttpPost]
        public async Task<IActionResult> AddTransportModeAsync(AddTransportModeVM addTransportVM)
        {
            var user = await usm.GetUserAsync(User);

            string RefN = "T" + Guid.NewGuid().ToString("N").Substring(0, 5);

            ModeOfTransport addThisTransport = new()
            {
                Name = addTransportVM.Name,

                Code = addTransportVM.Code,
                Description = addTransportVM.Description,
                CreatedBy = user?.UserName,
                CreatedDate = DateTime.UtcNow,
            };
       

            if (await saveHelper.SaveEntity(addThisTransport,user?.UserName))
            {
              

                return RedirectToAction("SystemSetup");
            }
      
      
     
                return RedirectToAction("SystemSetup");
            }
        


        [HttpPost]
        public async Task<IActionResult> AddTravelTypesAsync(AddTravelTypeVM addTravelTypeVM)
        {
            var user = await usm.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.UserName))
            {
           
                notyf.Error("User is not authenticated or user information is missing.", 5);
                return RedirectToAction("Login");
            }

            TravelType travelType = new()
            {
                Name = addTravelTypeVM.Name,
                Code = addTravelTypeVM.Code,
                Description = addTravelTypeVM.Description,
                CreatedBy = user?.UserName,
                CreatedDate = DateTime.UtcNow
            };

            if (await saveHelper.SaveEntity(travelType, user.UserName))
            {


                return RedirectToAction("SystemSetup");
            }

            return RedirectToAction("SystemSetup");
        }

        
    }
}






