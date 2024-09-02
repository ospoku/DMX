using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DMX.Data;
using DMX.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using DMX.Models;
using Microsoft.AspNetCore.Identity;
using DMX.ViewComponents;

namespace DMX.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SettingsController(XContext context, INotyfService notyfService, UserManager<AppUser>userManager) : Controller
    {
        public readonly XContext dcx = context;
        private readonly INotyfService notyf = notyfService;
        public readonly UserManager<AppUser>usm= userManager;

        public IActionResult Preferences()
        {

            return ViewComponent("ViewPreferences");
        }
        public IActionResult SystemSetup() 
        { 
            return ViewComponent("SystemSetup","ViewDepartments"); 
        }

        [HttpPost]
        public async Task<IActionResult> AddTravelTypeAsync(AddTravelTypeVM addTravelTypeVM)
        {
            var rand = new Random();
            int digit = 5;
            string RefN = "T" + rand.Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit));

          TravelType addThisTravelType = new()
            {
                Name = addTravelTypeVM.Name,

                Code = addTravelTypeVM.Code,
                Description = addTravelTypeVM.Description,
                CreatedBy = usm.GetUserAsync(User).Result.UserName,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.TravelTypes.Add(addThisTravelType);

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
        public async Task<IActionResult> AddFeeStructureAsync(AddFeeStructureVM addFeeStructureVM)
        {
            var rand = new Random();
            int digit = 5;
            string RefN = "F" + rand.Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit));

            FeeStructure addThisStructure = new()
            {
                Name = addFeeStructureVM.Name,

                MinDays = addFeeStructureVM.Min,
                MaxDays=addFeeStructureVM.Max,
                Fee= addFeeStructureVM.Fee,
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
            var rand = new Random();
            int digit = 5;
            string RefN = "D" + rand.Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit));

            DeceasedType addThisDeceasedType = new()
            {
                Name = addDeceasedTypeVM.Name,

                Code = addDeceasedTypeVM.Code,
                Description = addDeceasedTypeVM.Description,
                CreatedBy = usm.GetUserAsync(User).Result.UserName,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.DeceasedTypes.Add(addThisDeceasedType);

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
        public async Task<IActionResult> AddDepartmentAsync(AddDepartmentVM addDepartmentVM )
        {

            var rand = new Random();
            int digit = 5;
            string RefN = "D" + rand.Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit));

            Department addThisDepartment = new()
            {
                Name = addDepartmentVM.Name,

                Code = addDepartmentVM.Code,
                Description = addDepartmentVM.Description,
                CreatedBy = usm.GetUserAsync(User).Result.UserName,
                CreatedDate = DateTime.UtcNow,
            }; 
            dcx.Departments.Add(addThisDepartment);
           
            if (await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result.UserName) > 0)
            {
                notyf.Success("Record successfully saved", 5);

                return   RedirectToAction("SystemSetup");
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
       
    }
}






