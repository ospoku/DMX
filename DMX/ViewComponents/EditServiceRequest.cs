
using DMX.ViewModels;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DMX.ViewComponents
{
    public class EditServiceRequest(XContext dContext, IDataProtectionProvider provider) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly IDataProtector protector = provider.CreateProtector("IdProtector");

        public IViewComponentResult Invoke(string Id)
        {
          
            var decryptedId=protector.Unprotect(Id);
            if(!Guid.TryParse(decryptedId, out Guid requestGuid))
            {

            }

            ServiceRequest serviceRequestToEdit = new ServiceRequest();
            serviceRequestToEdit = (from sr in dcx.ServiceRequests.Include(sr => sr.Comments.OrderBy(m=>m.CreatedDate)) where sr.PublicId ==requestGuid select sr ).FirstOrDefault();

            EditServiceRequestVM editServiceRequestVM = new EditServiceRequestVM
            {
           


            };
            

            return View(editServiceRequestVM);
        }
    }
}
