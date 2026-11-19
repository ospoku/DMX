using AspNetCoreHero.ToastNotification.Abstractions;

using DMX.ViewModels;
using DMXApplication.Interfaces;
using DMXApplication.Services;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.Controllers
{
    public class DeceasedController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService, EmailService emailService,
          IEntityService entityService, AssignmentService assignmentService) : Controller
    {
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notify = notyfService;
        public readonly IEntityService entityServ = entityService;
        public readonly EmailService email = emailService;
       
        public readonly AssignmentService assignmentServ = assignmentService;
       

        [HttpGet]
        public IActionResult EditDeceased(Guid Id)
        {
            return ViewComponent(nameof(EditDeceased), Id);
        }

        public IActionResult ViewDeceases()
        {
            return ViewComponent(nameof(ViewDeceases));
        }

        public async Task<string> GetUserEmailAsync(string userId)
        {
            var user = await usm.FindByIdAsync(userId);
            return user?.Email;
        }

        [HttpGet]
        public IActionResult AddDeceased() => ViewComponent(nameof(AddDeceased));

        [HttpPost]
        public async Task<IActionResult> AddDeceased(AddDeceasedVM addDeceasedVM)
        {


            if (addDeceasedVM.SelectedUsers?.Any() != true)
            {
                notify.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction(nameof(ViewDeceases));
            }
            if (dcx.FeeStructures?.Any() != true)
            {
                notify.Error("You must set Fee structures", 5);
                return RedirectToAction(nameof(ViewDeceases));
            }

            try
            {
                var existingPatient = await dcx.Deceased.FirstOrDefaultAsync(p =>
                    p.Name.ToLower() == addDeceasedVM.DeceasedName.ToLower() &&
                    p.Depositor.ToLower() == addDeceasedVM.Depositor.ToLower());

                if (existingPatient != null)
                {
                    notify.Error("This record already exists.");
                    return RedirectToAction(nameof(ViewDeceases));
                }

                Deceased deceased = new()
                {
                    TagNo=addDeceasedVM.TagNo,
                    FolderNo=addDeceasedVM.FolderNo,
                    WardInCharge=addDeceasedVM.WardInCharge,
                    DeceasedTypeId=addDeceasedVM.DeceasedTypeId,
                    Description=addDeceasedVM.Description,
                    Depositor=addDeceasedVM.Depositor,
                    DepositorAddress=addDeceasedVM.DepositorAddress,
                    Diagnoses=addDeceasedVM.Diagnoses,
                    Name=addDeceasedVM.DeceasedName
                };

                entityServ.AddEntity(deceased);
                bool patientAdded = await entityServ.SaveEntity();
                if (!patientAdded)
                {
                    notify.Error("Failed to add patient.", 5);
                    return RedirectToAction("Error", "Home", new { message = "Patient creation failed." });
                }

                if (addDeceasedVM.SelectedUsers?.Any() == true)
                {
                    foreach (var userId in addDeceasedVM.SelectedUsers)
                    {
                        var assignment = new Assignment
                        {
                            //DeceasedId = deceased.DeceasedId,
                            //UserId = userId,
                        };

                         entityServ.AddEntity(assignment);
                        bool assignmentAdded = await entityServ.SaveEntity();
                        if (!assignmentAdded)
                        {
                            notify.Error("Failed to assign user.", 5);
                            return RedirectToAction("Error", "Home", new { message = "User assignment failed." });
                        }
                    }
                    notify.Success("Patient and assignments successfully processed.", 5);
                }
                else
                {
                    notify.Success("Patient created successfully, no users assigned.", 5);
                }

                return RedirectToAction(nameof(ViewDeceases));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                notify.Error("An unexpected error occurred.", 5);
                return RedirectToAction("Error", "Home", new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public IActionResult PrintPatient(Guid Id)
        {
            return ViewComponent(nameof(PrintPatient), Id);
        }

        [HttpGet]
        public IActionResult CommentDeceased(Guid Id)
        {
            return ViewComponent(nameof(CommentDeceased), Id);
        }

        [HttpGet]
        public IActionResult ViewInvoice(string Id)
        {
            return ViewComponent(nameof(ViewInvoice), Id);
        }
    }
}
