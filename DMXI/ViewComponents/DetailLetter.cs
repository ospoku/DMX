using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class DetailLetter : ViewComponent
    {
        public readonly XContext dcx;
        public readonly UserManager<AppUser> usm;
        public DetailLetter(XContext dContext, UserManager<AppUser> userManager)
        {
            dcx = dContext;
            usm = userManager;
        }
        public IViewComponentResult Invoke(string Id)
        {
             //var stringIDs = (from x in dcx.MemoAssignments where x.TaskId == Id select x.SelectedUsers).FirstOrDefault().Split(',');

            Letter documentDetail = new Letter();
            documentDetail = (from a in dcx.Letters where a.LetterId == Id & a.IsDeleted == false select a).FirstOrDefault();
            DetailLetterVM detailDocumentVM = new DetailLetterVM()
            {



                DocumentDate = documentDetail.DocumentDate,

                DocumentSource=documentDetail.Source,
                ReceiptDate=documentDetail.DateReceived,
                ReferenceNumber=documentDetail.ReferenceNumber,
                AdditionalNotes=documentDetail.AdditionalNotes,
               // SelectedUsers = (from x in dcx.MemoAssignments where x.TaskId == Id select x.SelectedUsers).FirstOrDefault().Split(','),
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),



            };
            return View(detailDocumentVM);
        }
    }
}
