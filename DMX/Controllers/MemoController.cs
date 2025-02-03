﻿using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMX.Controllers
{
    [Authorize]
    public class MemoController : Controller
    {
        private readonly XContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _notyfService;
        private readonly IAuthorizationService _authorizationService;
        private readonly EntityService _entityService;
        private readonly AssignmentService _assignmentService;

        public MemoController(
            XContext context,
            UserManager<AppUser> userManager,
            INotyfService notyfService,
            IAuthorizationService authorizationService,
            EntityService entityService,
            AssignmentService assignmentService)
        {
            _context = context;
            _userManager = userManager;
            _notyfService = notyfService;
            _authorizationService = authorizationService;
            _entityService = entityService;
            _assignmentService = assignmentService;
        }

        [HttpGet]
        public IActionResult ViewMemos() => ViewComponent("ViewMemos");

        [HttpGet]
        public IActionResult AddMemo() => ViewComponent("AddMemo");

        [HttpPost]
        public async Task<IActionResult> AddMemo(AddMemoVM addMemoVm)
        {
            if (addMemoVm.SelectedUsers == null || !addMemoVm.SelectedUsers.Any())
            {
                _notyfService.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewMemos");
            }

            try
            {
                var newMemo = new Memo
                {
                    Content = addMemoVm.Content,
                    Title = addMemoVm.Title
                };

                bool result = await _entityService.AddEntityAsync(newMemo, User);
                if (!result)
                {
                    _notyfService.Error("Failed to add the memo. Please try again.", 5);
                    return RedirectToAction("ViewMemos");
                }

                foreach (var userId in addMemoVm.SelectedUsers)
                {
                    var assignment = new MemoAssignment
                    {
                        MemoId = newMemo.MemoId,
                        UserId = userId
                    };

                    bool assignResult = await _assignmentService.AssignUsers(assignment, User);
                    if (!assignResult)
                    {
                        _notyfService.Error($"Failed to assign memo to user {userId}.", 5);
                    }
                }

                _notyfService.Success("Memo and assignments successfully processed.", 5);
                return RedirectToAction("ViewMemos");
            }
            catch (Exception ex)
            {
                _notyfService.Error("An error occurred: " + ex.Message, 5);
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the memo." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditMemoAsync(string id)
        {
            var decryptedId = Encryption.Decrypt(id);
            var memo = await _context.Memos.FirstOrDefaultAsync(m => m.MemoId == decryptedId);
            if (memo == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, memo, "MemoOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                _notyfService.Error("You do not have access to this resource!", 5);
                return Json(new { success = false, message = "You do not have access to this resource!" });
            }

            return ViewComponent("EditMemo", id);
        }

        [HttpPost]
        public async Task<IActionResult> EditMemo(string id, EditMemoVM editMemoVm)
        {
            if (editMemoVm.SelectedUsers == null || !editMemoVm.SelectedUsers.Any())
            {
                _notyfService.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewMemos");
            }

            try
            {
                var decryptedId = Encryption.Decrypt(id);
                var memoToUpdate = await _context.Memos.FirstOrDefaultAsync(m => m.MemoId == decryptedId);
                if (memoToUpdate == null)
                {
                    _notyfService.Error("Memo not found.", 5);
                    return RedirectToAction("ViewMemos");
                }

                memoToUpdate.Content = editMemoVm.Content;
                memoToUpdate.Title = editMemoVm.Title;

                bool isEdited = await _entityService.EditEntityAsync(memoToUpdate, User);
                if (!isEdited)
                {
                    _notyfService.Error("Failed to update memo. Please try again.", 5);
                    return RedirectToAction("ViewMemos");
                }

                var existingAssignments = _context.MemoAssignments.Where(a => a.MemoId == decryptedId);
                _context.MemoAssignments.RemoveRange(existingAssignments);

                bool atLeastOneFailed = false;
                var failedUsers = new List<string>();

                foreach (var userId in editMemoVm.SelectedUsers)
                {
                    var assignment = new MemoAssignment
                    {
                        MemoId = memoToUpdate.MemoId,
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

                return RedirectToAction("ViewMemos");
            }
            catch (Exception ex)
            {
                _notyfService.Error("An unexpected error occurred. Please try again.", 5);
                Console.WriteLine($"Error updating Memo: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the memo." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CommentMemo(string id, MemoCommentVM commentVm)
        {
            try
            {
                var decryptedId = Encryption.Decrypt(id);
                var memoToComment = await _context.Memos.FirstOrDefaultAsync(m => m.MemoId == decryptedId);
                if (memoToComment == null)
                {
                    return NotFound();
                }

                var newComment = new MemoComment
                {
                    MemoId = memoToComment.MemoId,
                    Message = commentVm.NewComment,
                    UserId = (await _userManager.GetUserAsync(User)).Id
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

                return RedirectToAction("ViewMemos");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request.", ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMemo(string id)
        {
            try
            {
                var decryptedId = Encryption.Decrypt(id);
                var memoToDelete = await _context.Memos.FirstOrDefaultAsync(m => m.MemoId == decryptedId);
                if (memoToDelete == null)
                {
                    return NotFound();
                }

                bool isDeleted = await _entityService.DeleteEntityAsync(memoToDelete, User);
                if (isDeleted)
                {
                    _notyfService.Success("Record successfully deleted", 5);
                }
                else
                {
                    _notyfService.Error("Record could not be deleted!!!", 5);
                }

                return RedirectToAction("ViewMemos");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request.", ex.Message });
            }
        }

        [HttpGet]
        public IActionResult CommentMemo(string id) => ViewComponent("CommentMemo", id);

        [HttpGet]
        public IActionResult PrintMemo(string id) => ViewComponent("PrintMemo", id);

        [HttpGet]
        public IActionResult DetailMemo(string id) => ViewComponent("DetailMemo", id);
   
    }
}