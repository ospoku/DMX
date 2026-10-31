using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.ViewComponents;
using DMX.ViewModels;
using DMXApplication.Services;
using DMXDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class MemoController(
    IMemoService memoService,
    INotyfService notifyService,
    IAuthorizationService authorizationService,
    IDataProtectionProvider provider,
    UserManager<AppUser> userManager) : Controller
{
    private readonly IMemoService _memoService = memoService;

    private readonly INotyfService _notifyService = notifyService;

    private readonly IAuthorizationService _authorizationService =
        authorizationService;

    private readonly UserManager<AppUser> _userManager = userManager;

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

            return RedirectToAction(nameof(ViewMemos));
        }

        try
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var dto = new MemoCreateDTO
            {
                Title = addMemoVm.Title,
                Content = addMemoVm.Content,
                SelectedUsers = addMemoVm.SelectedUsers
            };

            bool result = await _memoService.AddMemoAsync(dto, userId);

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
                !editMemoVm.SelectedUsers.Any())
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

                var userId = _userManager.GetUserId(User);

                if (string.IsNullOrEmpty(userId))
                    return Challenge();

                var dto = new MemoUpdateDTO
                {
                    PublicId = memoGuid,
                    Title = editMemoVm.Title,
                    Content = editMemoVm.Content,
                    SelectedUsers = editMemoVm.SelectedUsers
                };

                bool result = await _memoService.UpdateMemoAsync(dto, userId);

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
    }
            [HttpPost]
            public async Task<IActionResult> DeleteMemo(string id)
            {
                try
                {
                    var unprotectedId = _protector.Unprotect(id);

                    if (!Guid.TryParse(unprotectedId, out Guid memoGuid))
                        return BadRequest("Invalid memo ID.");

                    var userId = _userManager.GetUserId(User);

                    if (string.IsNullOrEmpty(userId))
                        return Challenge();

                    bool result = await _memoService.DeleteMemoAsync(
                        memoGuid, userId);

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

                var userId = _userManager.GetUserId(User);

                if (string.IsNullOrEmpty(userId))
                    return Challenge();

                var dto = new MemoCommentCreateDTO
                {
                    MemoPublicId = memoGuid,
                    Message = commentVm.NewComment,
                    UserId = userId
                };

                bool result = await _memoService.AddCommentAsync(dto, userId);

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
        } } }}