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
        UserManager<AppUser> userManager, IEntityService entityService) : Controller
    {
        private readonly IMemoService memoServ = memoService;

        private readonly INotyfService _notifyService = notifyService;
        private readonly IEntityService ens = entityService;
        private readonly IAuthorizationService _authorizationService =
            authorizationService;

        private readonly UserManager<AppUser> usm = userManager;

        private readonly IDataProtector _protector =
            provider.CreateProtector("IdProtector");


      
[HttpPost]
public async Task<IActionResult> AddMemo(AddMemoVM addMemoVm)
        {
            if (!ModelState.IsValid)
            {
             

                return View(addMemoVm);
            }

            if (addMemoVm.SelectedUsers == null ||
                addMemoVm.SelectedUsers.Count == 0)
            {
                _notifyService.Error(
                    "You must select at least one user for assignment.", 5);

                return View(addMemoVm);
            }

            try
            {
                var dto = new AddMemoDTO
                {
                    Title = addMemoVm.Title,
                    Content = addMemoVm.Content
                };

                // Create the memo and retrieve its ID
                bool memoId = await memoServ.AddMemo(dto);

                if (!memoId)
                {
                    _notifyService.Error("Memo could not be created.", 5);
                    return View(addMemoVm);
                }

                // Prepare the assignment DTO
                var assignDto = new AssignMemoDTO
                {
                    MemoId = dto.id,
                    SelectedUserIds = addMemoVm.SelectedUsers
                };

                // Assign the memo to the selected users
                bool assign = await assignServ.Assign(assignDto);

                if (!assign)
                {
                    _notifyService.Error(
                        "Memo was created, but assignment failed.", 5);

                    return RedirectToAction(nameof(ViewMemos));
                }

                _notifyService.Success(
                    "Memo successfully created and assigned.", 5);

                return RedirectToAction(nameof(ViewMemos));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                _notifyService.Error(
                    "An error occurred while creating the memo.", 5);

                return RedirectToAction("Error", "Home");
            }
        }



[HttpGet]
        public async Task<IActionResult> ViewMemos()
        {
            var memos = await memoServ.GetAllAsync();

            return ViewComponent(nameof(ViewMemos),memos);
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

                bool result = await ens.Update(dto);

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

                bool result = await ens.Delete(
                    new DeleteMemoDTO { PublicId = memoGuid });

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

                bool result = await memoServ.CommentMemo(dto);

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