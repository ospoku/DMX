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
using static DMX.Constants.Permissions;
using DMX.Helpers;

namespace DMX.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SettingsController(XContext context, INotyfService notyfService, UserManager<AppUser> userManager, SaveHelper saveHelper) : Controller
    {
        public readonly XContext dcx = context;
        public readonly INotyfService notyf = notyfService;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly SaveHelper saveHelper = saveHelper;

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
            var user = await usm.GetUserAsync(User);


            string RefN = "T" + Guid.NewGuid().ToString().Substring(0, 5);

            TravelType addThisTravelType = new()
            {
                Name = addTravelTypeVM.Name,

                Code = addTravelTypeVM.Code,
                Description = addTravelTypeVM.Description,
                CreatedBy = user?.UserName,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.TravelTypes.Add(addThisTravelType);

            if (await dcx.SaveChangesAsync(user?.UserName) > 0)
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
        public async Task<IActionResult> AddFeeStructureAsync(AddFeeStructureVM addFeeStructureVM)
        {
          
            string RefN = "F" + Guid.NewGuid().ToString().Substring(0, 5);

            FeeStructure addThisStructure = new()
            {
                Name = addFeeStructureVM.Name,

                MinDays = addFeeStructureVM.Min,
                MaxDays = addFeeStructureVM.Max,
                Fee = addFeeStructureVM.Fee,
                CreatedBy = usm.GetUserAsync(User).Result.UserName,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.FeeStructures.Add(addThisStructure);

            if (await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result.UserName) > 0)
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
        public async Task<IActionResult> AddDeceasedTypeAsync(AddDeceasedTypeVM addDeceasedTypeVM)
        {
         
            string RefN = "D" + Guid.NewGuid().ToString().Substring(0, 5);

            DeceasedType addThisDeceasedType = new()
            {
                Name = addDeceasedTypeVM.Name,

                Code = addDeceasedTypeVM.Code,
                Description = addDeceasedTypeVM.Description,
                CreatedBy = usm.GetUserAsync(User).Result?.UserName,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.DeceasedTypes.Add(addThisDeceasedType);

            if (await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result?.UserName) > 0)
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
        public async Task<IActionResult> AddDepartmentAsync(AddDepartmentVM addDepartmentVM)
        {

            var rand = new Random();
            int digit = 5;
            string RefN = "D" + rand.Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit));

            Department addThisDepartment = new()
            {
                Name = addDepartmentVM.Name,

                Code = addDepartmentVM.Code,
                Description = addDepartmentVM.Description,
                CreatedBy = usm.GetUserAsync(User).Result?.UserName,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.Departments.Add(addThisDepartment);

            if (await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result.UserName) > 0)
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

        [HttpGet]
        public IActionResult AddPerDiem()
        {
            return ViewComponent("AddPerDiem");
        }

        [HttpPost]
        public async Task<IActionResult> SavePCLimit(ViewPettyCashLimitVM cashLimitVM, PettyCashLimit cashLimit)
        {
            // Assuming you save the limit in a database or some other store

            PettyCashLimit limitToUpdate = (from a in dcx.PettyCashLimits where a.PettyCashLimitId == cashLimitVM.LimitId select a).FirstOrDefault();


            limitToUpdate.PettyCashLimitAmount = cashLimitVM.Amount;
            limitToUpdate.CreatedBy = usm.GetUserAsync(User).Result?.UserName;
            limitToUpdate.CreatedDate = DateTime.UtcNow;
            dcx.PettyCashLimits.Attach(limitToUpdate);
            dcx.Entry(limitToUpdate).State = EntityState.Modified;

            if (await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result.UserName) > 0)
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
            var rand = new Random();
            int digit = 5;
            string RefN = "T" + rand.Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit));

            ModeOfTransport addThisTransport = new()
            {
                Name = addTransportVM.Name,

                Code = addTransportVM.Code,
                Description = addTransportVM.Description,
                CreatedBy = usm.GetUserAsync(User)?.Result.UserName,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.ModesOfTransport.Add(addThisTransport);

            if (await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result.UserName) > 0)
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
        public async Task<IActionResult> AddTravelTypesAsync(AddTravelTypeVM addTravelTypeVM)
        {
            var user = await usm.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.UserName))
            {
                // Handle the scenario where the user is not authenticated or UserName is null
                notyf.Error("User is not authenticated or user information is missing.", 5);
                return RedirectToAction("Login");  // Or another action to handle unauthenticated users
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
                return RedirectToAction("SystemSetup");

            return RedirectToAction("SystemSetup");
        }

        
    }
}






