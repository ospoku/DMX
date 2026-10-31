using AspNetCoreHero.ToastNotification.Abstractions;
using DMXDomain.Models;
using DMX.ViewModels;
using DMXApplication.Services;
using DMXApplication.Services.DMXApplication.Services;
using DMXInfrastructure.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.Controllers
{
    public class PettyCashController(
        XContext context,
        UserManager<AppUser> userManager,
        INotyfService notifyService,
        IEntityService entityService,
        AssignmentService assignmentService, IDataProtectionProvider provider) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly XContext _context = context;
        private readonly INotyfService notyfService = notifyService;
        private readonly IEntityService ens = entityService;
        private readonly AssignmentService _assignmentService = assignmentService;
        public readonly IDataProtector protector = provider.CreateProtector("IdProtector");

        [HttpGet]
        public IActionResult ViewPettyCash() => ViewComponent(nameof(ViewPettyCash));

        [HttpGet]
        public IActionResult AddPettyCash() => ViewComponent(nameof(AddPettyCash));

        [HttpPost]
        public async Task<IActionResult> AddPettyCash(AddPettyCashVM addPettyCashVm)
        {
            if (addPettyCashVm.SelectedUsers == null || addPettyCashVm.SelectedUsers.Count == 0)
            {
                notyfService.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewPettyCash");
            }

            try
            {
                var newPettyCash = new PettyCash
                {
                    Amount = addPettyCashVm.Amount,
                    Purpose = addPettyCashVm.Purpose
                };

                bool result = await entityService.Add(newPettyCash, User);
                if (!result)
                {
                    notyfService.Error("An error occurred while processing the request.", 5);
                    return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request." });
                }

                foreach (var userId in addPettyCashVm.SelectedUsers)
                {
                    var assignment = new PettyCashAssignment
                    {
                        PettyCashId = newPettyCash.Id,
                        UserId = userId
                    };

                    bool assignResult = await ens.Add(assignment, User);
                    if (!assignResult)
                    {
                        notyfService.Error($"Failed to assign petty cash to user {userId}.", 5);
                    }
                }

                notyfService.Success("Petty cash and assignments successfully processed.", 5);
                return RedirectToAction("ViewPettyCash");
            }
            catch (Exception ex)
            {
                notyfService.Error("An error occurred while processing the request.", 5);
                return RedirectToAction("ViewPettyCash");
            }
        }

        [HttpGet]
        public IActionResult EditPettyCash(string id) => ViewComponent(nameof(EditPettyCash), id);

        [HttpPost]
        public async Task<IActionResult> EditPettyCash(EditPettyCashVM editPettyCashVm, string id)
        {
            if (editPettyCashVm.SelectedUsers == null || !editPettyCashVm.SelectedUsers.Any())
            {
                notyfService.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewPettyCash");
            }

            try
            {
                var decryptedId = protector.Unprotect(id);
                if(!Guid.TryParse(decryptedId, out Guid pettyCashGuid))
                {
                    notyfService.Error("Invalid petty cash ID format.", 5);
                    return RedirectToAction("ViewPettyCash");
                }
                var pettyCashToUpdate = await _context.PettyCash.FirstOrDefaultAsync(a => a.PublicId == pettyCashGuid);
                if (pettyCashToUpdate == null)
                {
                    notyfService.Error("Petty cash record not found.", 5);
                    return RedirectToAction("ViewPettyCash");
                }

                pettyCashToUpdate.Purpose = editPettyCashVm.Purpose;
                pettyCashToUpdate.Amount = editPettyCashVm.Amount;

                bool isEdited = await ens.Update(pettyCashToUpdate, User);
                if (!isEdited)
                {
                    notyfService.Error("Failed to update petty cash record. Please try again.", 5);
                    return RedirectToAction("ViewPettyCash");
                }

                var existingAssignments = _context.PettyCashAssignments.Where(x => x.PublicId == pettyCashGuid);
                _context.PettyCashAssignments.RemoveRange(existingAssignments);

                bool atLeastOneFailed = false;
                var failedUsers = new List<string>();

                foreach (var userId in editPettyCashVm.SelectedUsers)
                {
                    var assignment = new PettyCashAssignment
                    {
                        PettyCashId = pettyCashToUpdate.Id,
                        UserId = userId
                    };

                    bool assignResult = await _assignmentService.AssignUsers(assignment, User);
                    if (!assignResult)
                    {
                        atLeastOneFailed = true;
                        failedUsers.Add(userId);
                    }
                }

                if (atLeastOneFailed)
                {
                    notyfService.Warning($"Record updated, but some assignments failed: {string.Join(", ", failedUsers)}", 7);
                }
                else
                {
                    notyfService.Success("Record successfully updated", 5);
                }

                return RedirectToAction(nameof(ViewPettyCash));
            }
            catch (Exception ex)
            {
                notyfService.Error("An unexpected error occurred. Please try again.", 5);
                Console.WriteLine($"Error updating petty cash: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the petty cash record." });
            }
        }

        [HttpGet]
        public IActionResult CommentPettyCash(string id) => ViewComponent(nameof(CommentPettyCash), id);

        [HttpPost]
        public async Task<IActionResult> CommentPettyCash(string id, PettyCashCommentVM commentVm)
        {
            try
            {
                var decryptedId = protector.Unprotect(id);
                if(!Guid.TryParse(decryptedId, out Guid decryptedIdGuid))
                {
                    return NotFound();
                }
                var pettyCashToComment = await _context.PettyCash.FirstOrDefaultAsync(a => a.PublicId == decryptedIdGuid);
                if (pettyCashToComment == null)
                {
                    return NotFound();
                }

                var newComment = new PettyCashComment
                {
                    PettyCashId = pettyCashToComment.Id,
                    Message = commentVm.NewComment,
                    UserId = (await _userManager.GetUserAsync(User)).Id,
                    CreatedDate = DateTime.Now
                };

                bool result = await ens.Add(newComment, User);
                if (result)
                {
                    notyfService.Success("Comment successfully saved", 5);
                }
                else
                {
                    notyfService.Error("Comment could not be saved!!!", 5);
                }

                return RedirectToAction(nameof(ViewPettyCash));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request.", ex.Message });
            }
        }
    }
}