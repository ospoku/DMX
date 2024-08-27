using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DMX.Data;
using DMX.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using DMX.Models;
using Microsoft.AspNetCore.Identity;

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
        public IActionResult EditTier1(AddTier1VM addTier1VM)
        {
            
            
                notyf.Success("This is Edit Button", 5);

                notyf.Error("Document saving failed");
                

   
                notyf.Success("Record successfully saved!!!", 5);

               
            
            return View();

        }
        [HttpPost]
        public IActionResult SaveTier1(AddTier1VM addTier1VM)
        {


            notyf.Success("This is Edit Button", 5);

            notyf.Error("Document saving failed");



            notyf.Success("Record successfully saved!!!", 5);



            return View();

        }
        [HttpPost]
        public IActionResult EditTier2(AddTier1VM addTier1VM)
        {


            notyf.Success("This is Edit Button", 5);

            notyf.Error("Document saving failed");



            notyf.Success("Record successfully saved!!!", 5);



            return View("/SystemSetup");

        }
        [HttpPost]
        public IActionResult SaveTier2(AddTier1VM addTier1VM)
        {


            notyf.Success("This is Edit Button", 5);

            notyf.Error("Document saving failed");



            notyf.Success("Record successfully saved!!!", 5);



            return View("/SystemSetup");

        }
        [HttpGet]
        public IActionResult AddTravelType()
        {
            return ViewComponent("AddTravelType");
        }
        [HttpGet]
        public IActionResult AddDeceasedType()
        {
            return ViewComponent("AddDeceasedType");
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






