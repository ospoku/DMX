using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    
    public class EditClassroom(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        
        public IViewComponentResult Invoke(string Id)
        {

              
                Memo memoToEdit = new();
                memoToEdit = (from m in dcx.Memos where m.MemoId == @Encryption.Decrypt(Id) select m).FirstOrDefault();

                EditMemoVM editMemoVM = new()
                {

                    Title = memoToEdit.Title,
                    Content = memoToEdit.Content,
                    SelectedUsers = (from x in dcx.MemoAssignments where x.MemoId == @Encryption.Decrypt(Id) select x.UserId).ToList(),
                    UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),
                };


                return View(editMemoVM);
            }
          

        }
    }


