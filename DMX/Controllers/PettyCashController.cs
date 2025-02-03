using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMX.Controllers
{
    public class PettyCashController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly XContext _context;
        private readonly INotyfService _notyfService;
        private readonly EntityService _entityService;
        private readonly AssignmentService _assignmentService;

        public PettyCashController(
            XContext context,
            UserManager<AppUser> userManager,
            INotyfService notyfService,
            EntityService entityService,
            AssignmentService assignmentService)
        {
            _userManager = userManager;
            _context = context;
            _notyfService = notyfService;
            _entityService = entityService;
            _assignmentService = assignmentService;
        }

        [HttpGet]
        public IActionResult ViewPettyCash() => ViewComponent("ViewPettyCash");

        [HttpGet]
        public IActionResult AddPettyCash() => ViewComponent("AddPettyCash");

        [HttpPost]
        public async Task<IActionResult> AddPettyCash(AddPettyCashVM addPettyCashVm)
        {
            if (addPettyCashVm.SelectedUsers == null || !addPettyCashVm.SelectedUsers.Any())
            {
                _notyfService.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewPettyCash");
            }

            try
            {
                var newPettyCash = new PettyCash
                {
                    Amount = addPettyCashVm.Amount,
                    Purpose = addPettyCashVm.Purpose
                };

                bool result = await _entityService.AddEntityAsync(newPettyCash, User);
                if (!result)
                {
                    _notyfService.Error("An error occurred while processing the request.", 5);
                    return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request." });
                }

                foreach (var userId in addPettyCashVm.SelectedUsers)
                {
                    var assignment = new PettyCashAssignment
                    {
                        PettyCashId = newPettyCash.PettyCashId,
                        UserId = userId
                    };

                    bool assignResult = await _entityService.AddEntityAsync(assignment, User);
                    if (!assignResult)
                    {
                        _notyfService.Error($"Failed to assign petty cash to user {userId}.", 5);
                    }
                }

                _notyfService.Success("Petty cash and assignments successfully processed.", 5);
                return RedirectToAction("ViewPettyCash");
            }
            catch (Exception ex)
            {
                _notyfService.Error("An error occurred while processing the request.", 5);
                return RedirectToAction("ViewPettyCash");
            }
        }

        [HttpGet]
        public IActionResult EditPettyCash(string id) => ViewComponent("EditPettyCash", id);

        [HttpPost]
        public async Task<IActionResult> EditPettyCash(EditPettyCashVM editPettyCashVm, string id)
        {
            if (editPettyCashVm.SelectedUsers == null || !editPettyCashVm.SelectedUsers.Any())
            {
                _notyfService.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewPettyCash");
            }

            try
            {
                var decryptedId = Encryption.Decrypt(id);
                var pettyCashToUpdate = await _context.PettyCash.FirstOrDefaultAsync(a => a.PettyCashId == decryptedId);
                if (pettyCashToUpdate == null)
                {
                    _notyfService.Error("Petty cash record not found.", 5);
                    return RedirectToAction("ViewPettyCash");
                }

                pettyCashToUpdate.Purpose = editPettyCashVm.Purpose;
                pettyCashToUpdate.Amount = editPettyCashVm.Amount;

                bool isEdited = await _entityService.EditEntityAsync(pettyCashToUpdate, User);
                if (!isEdited)
                {
                    _notyfService.Error("Failed to update petty cash record. Please try again.", 5);
                    return RedirectToAction("ViewPettyCash");
                }

                var existingAssignments = _context.PettyCashAssignments.Where(x => x.PettyCashId == decryptedId);
                _context.PettyCashAssignments.RemoveRange(existingAssignments);

                bool atLeastOneFailed = false;
                var failedUsers = new List<string>();

                foreach (var userId in editPettyCashVm.SelectedUsers)
                {
                    var assignment = new PettyCashAssignment
                    {
                        PettyCashId = pettyCashToUpdate.PettyCashId,
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
                    _notyfService.Warning($"Record updated, but some assignments failed: {string.Join(", ", failedUsers)}", 7);
                }
                else
                {
                    _notyfService.Success("Record successfully updated", 5);
                }

                return RedirectToAction("ViewPettyCash");
            }
            catch (Exception ex)
            {
                _notyfService.Error("An unexpected error occurred. Please try again.", 5);
                Console.WriteLine($"Error updating petty cash: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the petty cash record." });
            }
        }

        [HttpGet]
        public IActionResult CommentPettyCash(string id) => ViewComponent("CommentPettyCash", id);

        [HttpPost]
        public async Task<IActionResult> CommentPettyCash(string id, PettyCashCommentVM commentVm)
        {
            try
            {
                var decryptedId = Encryption.Decrypt(id);
                var pettyCashToComment = await _context.PettyCash.FirstOrDefaultAsync(a => a.PettyCashId == decryptedId);
                if (pettyCashToComment == null)
                {
                    return NotFound();
                }

                var newComment = new PettyCashComment
                {
                    PettyCashId = pettyCashToComment.PettyCashId,
                    Message = commentVm.NewComment,
                    UserId = (await _userManager.GetUserAsync(User)).Id,
                    CreatedDate = DateTime.Now
                };

                bool result = await _entityService.AddEntityAsync(newComment, User);
                if (result)
                {
                    _notyfService.Success("Comment successfully saved", 5);
                }
                else
                {
                    _notyfService.Error("Comment could not be saved!!!", 5);
                }

                return RedirectToAction("ViewPettyCash");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request.", ex.Message });
            }
        }
    }
}