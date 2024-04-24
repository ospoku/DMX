using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewWorkshops(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {


            var documentList = dcx.Letters.Where(d => d.IsDeleted == false).Select(d => new ViewDocumentsVM
            {

                LetterId = d.LetterId,
                AdditionalNotes = d.AdditionalNotes,
                DocumentSource = d.Source,
                DocumentDate = d.DocumentDate,
                ReferenceNumber = d.ReferenceNumber,
                ReceiptDate = d.DateReceived,
               
               

            }).ToList();

           
            return View(documentList);
        }
    }
}
