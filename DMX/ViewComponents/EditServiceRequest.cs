using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DMX.ViewComponents
{
    public class EditServiceRequest:ViewComponent
    {
        public readonly XContext dcx;
        public EditServiceRequest(XContext dContext)
        {
            dcx = dContext;
        }

        public IViewComponentResult Invoke(string Id)
        {
           var decodedId=HttpUtility.UrlDecode(Id)?.Replace(" ","+");
            var decryptedId=Encryption.Decrypt(decodedId);
            if(!Guid.TryParse(decryptedId, out Guid requestGuid))
            {

            }

            ServiceRequest serviceRequestToEdit = new ServiceRequest();
            serviceRequestToEdit = (from sr in dcx.ServiceRequests.Include(sr => sr.Comments.OrderBy(m=>m.CreatedDate)) where sr.RequestId ==requestGuid select sr ).FirstOrDefault();

            EditServiceRequestVM editServiceRequestVM = new EditServiceRequestVM
            {
           


            };
            

            return View(editServiceRequestVM);
        }
    }
}
