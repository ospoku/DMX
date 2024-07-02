using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public IActionResult DetailExcuseDuty(string Id)
        {
            return ViewComponent("DetailExcuseDuty", Id);
        }
        [HttpPost]
        public async Task<IActionResult> CommentExcuseDuty(string Id, MemoCommentVM addCommentVM)
        {

            ExcuseDuty dutyToComment = new();
            dutyToComment = (from a in dcx.ExcuseDuties where a.Id == Id select a).FirstOrDefault();

            ExcuseDutyComment addThisComment = new()
            {
                ExcuseDutyId = dutyToComment.Id,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                 UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.ExcuseDutyComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewExcuseDuties");
        }
        [Authorize(Policy ="OwnerPolicy")]

        [HttpGet]
        public IActionResult EditExcuseDuty(string Id)
        {
            return ViewComponent("EditExcuseDuty", Id);
        }
        [HttpPost]
        public async Task<IActionResult> AddExcuseDuty(AddExcuseDutyVM addExcuseDutyVM)
        {
            var rand = new Random();
            int digit = 5;
            string RefN = "E" + rand.Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit));
            ExcuseDuty addThisExcuseDuty = new()
            {
                Date = addExcuseDutyVM.Date,
                DateofDischarge = addExcuseDutyVM.DateofDischarge,
                ExcuseDays = addExcuseDutyVM.ExcuseDays,
                ReferenceNumber = RefN,
                OperationDiagnosis = addExcuseDutyVM.OperationDiagnosis,
                CreatedBy = usm.GetUserAsync(HttpContext.User).Result.UserName,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.ExcuseDuties.Add(addThisExcuseDuty);
            foreach (var user in addExcuseDutyVM.SelectedUsers)
            {
                dcx.ExcuseDutyAssignments.Add(
                    new ExcuseDutyAssignment
                    {
                        ExcuseDutyId = addThisExcuseDuty.Id,
                        AppUserId = user,
                        CreatedBy = usm.GetUserAsync(HttpContext.User).Result.UserName,
                        CreatedDate = DateTime.UtcNow
                    });
            };

           
            if (await dcx.SaveChangesAsync( usm.GetUserAsync(HttpContext.User).Result.UserName) > 0)
            {
                notyf.Success("Excuse Duty successfully saved", 5);
                return RedirectToAction("ViewExcuseDuties");

            }
            else
            {
                notyf.Error("Error, Excuse Duty could not be saved!!!", 5);
            }

            return ViewComponent("AddExcuseDuty");
        }
        [HttpPost]
        public async Task<IActionResult> EditExcuseDutyAsync(string Id, EditExcuseDutyVM editExcuseDutyVM)
        {

            ExcuseDuty updateThisExcuseDuty = dcx.ExcuseDuties.Where(e => e.Id == @Encryption.Decrypt(Id)).Select(e => e).FirstOrDefault();


           
            updateThisExcuseDuty.DateofDischarge = editExcuseDutyVM.DateofDischarge;
            updateThisExcuseDuty.ExcuseDays = editExcuseDutyVM.ExcuseDays;

            updateThisExcuseDuty.OperationDiagnosis = editExcuseDutyVM.OperationDiagnosis;

            updateThisExcuseDuty.ModifiedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value;


            foreach (var assignment in dcx.MemoAssignments.Where(a => a.MemoId == @Encryption.Decrypt(Id)))
            {
                dcx.MemoAssignments.Remove(assignment);
            };
            dcx.SaveChanges();
            dcx.ExcuseDuties.Attach(updateThisExcuseDuty);

            dcx.Entry(updateThisExcuseDuty).State = EntityState.Modified;

            foreach (var user in editExcuseDutyVM.SelectedUsers)
            {
               dcx.ExcuseDutyAssignments.Add(new ExcuseDutyAssignment
               {
                    ExcuseDutyId = @Encryption.Decrypt(Id),
                    AppUserId = user,
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

        [HttpGet]
        public IActionResult AddExcuseDuty()
        {
            return ViewComponent("AddExcuseDuty");
        }



    }
}
