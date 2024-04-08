using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.Controllers
{
    public class ExcuseDutyController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService) : Controller
    {

        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;
        [HttpGet]
        public IActionResult ViewExcuseDuties()
        {
            return ViewComponent("ViewExcuseDuties");
        }

        [HttpGet]
        public IActionResult EditExcuseDuty(string Id)
        {
            return ViewComponent("EditExcuseDuty", Id);
        }
        [HttpPost]
        public async Task<IActionResult> AddExcuseDuty(AddExcuseDutyVM addExcuseDutyVM)
        {
            ExcuseDuty addThisExcuseDuty = new()
            {
                Date = addExcuseDutyVM.Date,
                DateofDischarge = addExcuseDutyVM.DateofDischarge,
                ExcuseDays = addExcuseDutyVM.ExcuseDays,
                Name = addExcuseDutyVM.Name,
                OperationDiagnosis = addExcuseDutyVM.OperationDiagnosis,
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.ExcuseDuties.Add(addThisExcuseDuty);

            dcx.Assignments.Add(new Assignment
            {
                TaskId = addThisExcuseDuty.ExcuseFormId,
                SelectedUsers = string.Join(',', addExcuseDutyVM.SelectedUsers),
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                CreatedDate = DateTime.UtcNow,
            });
            if (await dcx.SaveChangesAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Memo successfully saved", 5);


            }
            else
            {
                notyf.Error("Error, Memo could not be saved!!!", 5);
            }

            return ViewComponent("ViewExcuseDuties");
        }
        [HttpPost]
        public async Task<IActionResult> EditExcuseDutyAsync(string Id, EditExcuseDutyVM editExcuseDutyVM)
        {

            ExcuseDuty updateThisExcuseDuty = dcx.ExcuseDuties.Where(e => e.ExcuseFormId == @Encryption.Decrypt(Id)).Select(e => e).FirstOrDefault();


            updateThisExcuseDuty.Name = editExcuseDutyVM.Name;
            updateThisExcuseDuty.DateofDischarge = editExcuseDutyVM.DateofDischarge;
            updateThisExcuseDuty.ExcuseDays = editExcuseDutyVM.ExcuseDays;

            updateThisExcuseDuty.OperationDiagnosis = editExcuseDutyVM.OperationDiagnosis;

            updateThisExcuseDuty.ModifiedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value;


            foreach (var assignment in dcx.Assignments.Where(a => a.TaskId == @Encryption.Decrypt(Id)))
            {
                dcx.Assignments.Remove(assignment);
            };
            dcx.SaveChanges();
            dcx.ExcuseDuties.Attach(updateThisExcuseDuty);

            dcx.Entry(updateThisExcuseDuty).State = EntityState.Modified;

            foreach (var user in editExcuseDutyVM.SelectedUsers)
            {
                dcx.Assignments.Add(new Assignment
                {
                    TaskId = @Encryption.Decrypt(Id),
                    SelectedUsers = user,
                    ModifiedDate = DateTime.Now,
                });
            }
            if (await dcx.SaveChangesAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Record successfully updated");

                return RedirectToAction("ViewExcuseDuties");
            }
            else
            {
                return ViewComponent("EditExcuseDuty");
            }

        }


     
    }
}
