using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.ViewComponents;
using DMX.ViewModels;
using DMXApplication.DTO;
using DMXApplication.DTO.Memos;
using DMXApplication.Interfaces;
using DMXApplication.Services;
using DMXDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    [Authorize]
    public class MemoController(
        IMemoService memoService,
        INotyfService notifyService,
        IAuthorizationService authorizationService,
        IDataProtectionProvider provider,
        UserManager<AppUser> userManager) : Controller
    {
        private readonly IMemoService memoServ = memoService;

        private readonly INotyfService _notifyService = notifyService;

        private readonly IAuthorizationService _authorizationService =
            authorizationService;

        private readonly UserManager<AppUser> usm = userManager;

        private readonly IDataProtector _protector =
            provider.CreateProtector("IdProtector");


        [HttpPost]
        public async Task<IActionResult> AddMemo(AddMemoVM addMemoVm)
        {
            if (!ModelState.IsValid)
                return View(addMemoVm);

            if (addMemoVm.SelectedUsers == null ||
                addMemoVm.SelectedUsers.Count == 0)
            {
                _notifyService.Error(
                    "You must select at least one user for assignment.", 5);

                return View(addMemoVm);
            }

            try
            {
                var userId = usm.GetUserId(User);

                if (string.IsNullOrEmpty(userId))
                    return Challenge();

                var dto = new AddMemoDTO
                {
                    Title = addMemoVm.Title,
                    Content = addMemoVm.Content,
                    SelectedUserIds = addMemoVm.SelectedUsers
                };

                bool result = await memoServ.AddMemo(dto, userId);

                if (result)
                    _notifyService.Success("Memo successfully created.", 5);
                else
                    _notifyService.Error("Memo could not be created.", 5);

                return RedirectToAction(nameof(ViewMemos));
            }
            catch (Exception)
            {
                _notifyService.Error(
                    "An error occurred while creating the memo.", 5);

                return RedirectToAction("Error", "Home");
            }
            

            
        }
        [HttpPost]
        public async Task<IActionResult> ViewMemos(ViewMemosVM viewMemosVM)
        {
            var dto = new ViewMemosDTO
            {
                Message=viewMemosVM.
            };

            return memoServ.GetAllAsync(dto,);
        }


        [HttpPost]
        public async Task<IActionResult> EditMemo(
        string id,
        EditMemoVM editMemoVm)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Please correct the submitted information.", 5);
                return RedirectToAction(nameof(ViewMemos));
            }

            if (editMemoVm.SelectedUsers == null ||
                editMemoVm.SelectedUsers.Count == 0)
            {
                _notifyService.Error(
                    "You must select at least one user for assignment.", 5);

                return RedirectToAction(nameof(ViewMemos));
            }

            try
            {

                var unprotectedId = _protector.Unprotect(id);

                if (!Guid.TryParse(unprotectedId, out Guid memoGuid))
                    return BadRequest("Invalid memo ID.");

                var userId = usm.GetUserId(User);

                if (string.IsNullOrEmpty(userId))
                    return Challenge();

                var dto = new UpdateMemoDTO
                {
                    PublicId = memoGuid,
                    Title = editMemoVm.Title,
                    Content = editMemoVm.Content,
                    SelectedUserIds = editMemoVm.SelectedUsers
                };

                bool result = await memoServ.UpdateMemo(dto, userId);

                if (result)
                    _notifyService.Success("Memo successfully updated.", 5);
                else
                    _notifyService.Error("Memo could not be updated.", 5);

                return RedirectToAction(nameof(ViewMemos));
            }
            catch (Exception)
            {
                _notifyService.Error("An error occurred while updating the memo.", 5);
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteMemo(string id)
        {
            try
            {
                var unprotectedId = _protector.Unprotect(id);

                if (!Guid.TryParse(unprotectedId, out Guid memoGuid))
                    return BadRequest("Invalid memo ID.");

                var userId = usm.GetUserId(User);

                if (string.IsNullOrEmpty(userId))
                    return Challenge();

                bool result = await memoServ.DeleteMemo(
                    new DeleteMemoDTO { PublicId = memoGuid }, userId);

                if (result)
                    _notifyService.Success("Record successfully deleted.", 5);
                else
                    _notifyService.Error("Memo could not be deleted.", 5);

                return RedirectToAction(nameof(ViewMemos));
            }
            catch (Exception)
            {
                _notifyService.Error(
                    "An error occurred while deleting the memo.", 5);

                return RedirectToAction(nameof(ViewMemos));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CommentMemo(
    string id,
    MemoCommentVM commentVm)
        {
            try
            {
                var unprotectedId = _protector.Unprotect(id);

                if (!Guid.TryParse(unprotectedId, out Guid memoGuid))
                    return BadRequest("Invalid memo ID.");

                var userId = usm.GetUserId(User);

                if (string.IsNullOrEmpty(userId))
                    return Challenge();

                var dto = new CommentMemoDTO
                {
                    PublicId = memoGuid,
                    Message = commentVm.NewComment,
                    UserId = userId
                };

                bool result = await memoServ.CommentMemo(dto, userId);

                if (result)
                    _notifyService.Success("Comment successfully saved.", 5);
                else
                    _notifyService.Error("Comment could not be saved.", 5);

                return RedirectToAction(nameof(ViewMemos));
            }
            catch (Exception)
            {
                _notifyService.Error(
                    "An error occurred while saving the comment.", 5);

                return RedirectToAction(nameof(ViewMemos));
            }
        }
    }
}