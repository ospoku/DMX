using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DMX.ViewComponents
{

    public class EditMemo(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
<<<<<<< HEAD
        
        public IViewComponentResult Invoke(Guid Id)
=======

        public IViewComponentResult Invoke(string Id)
>>>>>>> 1454a21726b35c461febf31d14b931aa5002a26b
        {
            var decodedId = HttpUtility.UrlDecode(Id)?.Replace(" ", "+"); // sanitize
            var decryptedId = Encryption.Decrypt(decodedId);
            if (!Guid.TryParse(decryptedId, out Guid memoGuid))
            {   return View("BadRequest", "Invalid memo ID format."); }
                Memo memoToEdit = new();
                memoToEdit = (from m in dcx.Memos where m.MemoId == memoGuid select m).FirstOrDefault();
                EditMemoVM editMemoVM = new()
                {
                    Title = memoToEdit.Title,
                    Content = memoToEdit.Content,
                    SelectedUsers = (from x in dcx.MemoAssignments where x.MemoId == memoGuid select x.UserId).ToList(),
                    UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id), nameof(AppUser.Fullname))),
                };




                return View(editMemoVM);
            }


        }
    }



