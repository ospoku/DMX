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
    public class SettingsController(XContext context, INotyfService notyfService, UserManager<AppUser> userManager, EntityService entityService) : Controller
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

        //[HttpPost]
        //public async Task<IActionResult> AddTravelTypeAsync(AddTravelTypeVM addTravelTypeVM)
        //{
           

        //    TravelType travelType = new()
        //    {
        //        Name = addTravelTypeVM.Name,
        //        Code = addTravelTypeVM.Code,
        //        Description = addTravelTypeVM.Description,
        //    };

        //    if (await entityServ.AddEntityAsync(travelType, User))
        //    {
        //        return RedirectToAction("SystemSetup");
        //    }
        //    return RedirectToAction("SystemSetup");

        //}
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

            };
            // Call the service method, which returns a bool
            bool result = await entityServ.AddEntityAsync(addThisStructure, User);

            // Based on the result, redirect or return the appropriate response
            if (result)
            {
                // Success: Redirect to SystemSetup
                return RedirectToAction("SystemSetup");
            }
            else
            {
                // Failure: Return an error view or handle as needed
                return RedirectToAction("SystemSetup"); // You can return an error page if preferred
            }
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
                Description = addDeceasedTypeVM.Description
            };
            // Call the service method, which returns a bool
            bool result = await entityServ.AddEntityAsync(addThisDeceasedType, User);

            // Based on the result, redirect or return the appropriate response
            if (result)
            {
                // Success: Redirect to SystemSetup
                return RedirectToAction("SystemSetup");
            }
            else
            {
                // Failure: Return an error view or handle as needed
                return RedirectToAction("SystemSetup"); // You can return an error page if preferred
            }
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
                Description = addDepartmentVM.Description
            };
            // Call the service method, which returns a bool
            bool result = await entityServ.AddEntityAsync(addThisDepartment, User);

            // Based on the result, redirect or return the appropriate response
            if (result)
            {
                // Success: Redirect to SystemSetup
                return RedirectToAction("SystemSetup");
            }
            else
            {
                // Failure: Return an error view or handle as needed
                return RedirectToAction("SystemSetup"); // You can return an error page if preferred
            }
        }


        [HttpGet]
        public IActionResult AddPerDiem()
        {
            return ViewComponent("AddPerDiem");
        }
        [HttpPost]
        public async Task<IActionResult> AddCashLimit(EditLimitVM limitVM, CashLimit cashLimit)
        {

            // Assuming you save the limit in a database or some other store

            CashLimit addThisLimit = new()
            {



                Amount = cashLimit.Amount,
            };

            bool result = await entityServ.AddEntityAsync(addThisLimit, User);

            // Based on the result, redirect or return the appropriate response
            if (result)
            {
                // Success: Redirect to SystemSetup
                return RedirectToAction("SystemSetup");
            }
            else
            {
                // Failure: Return an error view or handle as needed
                return RedirectToAction("SystemSetup"); // You can return an error page if preferred
            }

        }
        //[HttpPost]
        //public async Task<IActionResult> EditCashLimit(EditLimitVM limitVM, CashLimit cashLimit)
        //{
        //    var user = await usm.GetUserAsync(User);
        //    // Assuming you save the limit in a database or some other store

        //    CashLimit limitToUpdate = (from a in dcx.CashLimits where a.CashLimitId == cashLimit.CashLimitId select a).FirstOrDefault();


        //    limitToUpdate.Amount = cashLimit.Amount;
        //    limitToUpdate.CreatedBy = user?.UserName;
        //    limitToUpdate.CreatedDate = DateTime.UtcNow;
        //    dcx.CashLimits.Attach(limitToUpdate);
        //    dcx.Entry(limitToUpdate).State = EntityState.Modified;

        //    if (await saveHelper.SaveEntity(limitToUpdate, user?.UserName))
        //    {
        //        notyf.Success("Record successfully saved", 5);

        //        return RedirectToAction("SystemSetup");
        //    }
        //    else
        //    {
        //        notyf.Error("Error, Record could not be saved!!!", 5);
        //        return RedirectToAction("SystemSetup");
        //    }


        //}
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
            };
            // Call the service method, which returns a bool
            bool result = await entityServ.AddEntityAsync(addThisTransport, User);

            // Based on the result, redirect or return the appropriate response
            if (result)
            {
                // Success: Redirect to SystemSetup
                return RedirectToAction("SystemSetup");
            }
            else
            {
                // Failure: Return an error view or handle as needed
                return RedirectToAction("SystemSetup"); // You can return an error page if preferred
            }

        }


        [HttpPost]
        public async Task<IActionResult> AddTravelTypeAsync(AddTravelTypeVM addTravelTypeVM)
        {
            TravelType travelType = new()
            {
                Name = addTravelTypeVM.Name,
                Code = addTravelTypeVM.Code,
                Description = addTravelTypeVM.Description,
            };

            // Call the service method, which returns a bool
            bool result = await entityServ.AddEntityAsync(travelType, User);

            // Based on the result, redirect or return the appropriate response
            if (result)
            {
                // Success: Redirect to SystemSetup
                return RedirectToAction("SystemSetup");
            }
            else
            {
                // Failure: Return an error view or handle as needed
                return RedirectToAction("SystemSetup"); // You can return an error page if preferred
            }
        }
    }


}







