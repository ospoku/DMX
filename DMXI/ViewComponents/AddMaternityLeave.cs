using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class AddDocument : ViewComponent
    {
        public readonly UserManager<AppUser> usm;
  
        public AddDocument(UserManager<AppUser> userManager)
        {
            usm = userManager;

        }

        public IViewComponentResult Invoke()
        {
            AddLetterVM addDocumentVM = new ()
            {

                UsersList = new SelectList(usm.Users.ToList(), "Id","UserName"),


            };

            return View(addDocumentVM);
        }
            
    }
}
