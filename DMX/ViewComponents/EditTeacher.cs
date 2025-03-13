using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class EditTeacher(UserManager<AppUser> userManager, XContext dContext) :ViewComponent
    {
        public readonly XContext dcx=dContext;

        public readonly UserManager<AppUser> usm = userManager;
        public IViewComponentResult Invoke(string Id)

        {
            
          var  teacherToEdit = (from m in dcx.Teachers where m.TeacherId == @Encryption.Decrypt(Id) select m ).FirstOrDefault();

            EditTeacherVM editDeceasedVM = new EditTeacherVM
            {
            
           

            };
            

            return View(editDeceasedVM);
        }
    }
}
