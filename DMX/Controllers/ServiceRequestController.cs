using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class ServiceRequestController( XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService
           ) : Controller
    {
        public readonly UserManager<AppUser> usm = userManager;
    public readonly XContext dcx = dContext;
    private readonly INotyfService notyf = notyfService;
    
        public IActionResult ViewServiceRequests()
        {
            return ViewComponent("ViewServiceRequests");
        }
        [HttpGet]
        public IActionResult AddServiceRequest()
        {
            return ViewComponent("AddServiceRequest");
        }
        [HttpPost]
        public async Task<IActionResult> ServiceRequestComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            ServiceRequestComment addThisComment = new()
            {
                ServiceRequestId = memoToUpdate.MemoId,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                //  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.ServiceRequestComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }

        [HttpPost]
        public async Task<IActionResult> AddServiceRequest(AddServiceRequestVM addServiceRequestVM)
        {
            ServiceRequest addThisServiceRequest = new()
            {
                ActionToBeTaken = addServiceRequestVM.ActionToBeTaken,
                FaultInspectedBy = addServiceRequestVM.FaultInspectedBy,
                Faults = addServiceRequestVM.Faults,
            };
            dcx.ServiceRequests.Add(addThisServiceRequest);

            if (await dcx.SaveChangesAsync() > 0)
            {
                return RedirectToAction("ViewServiceRequests");
            }
            else
            {
                return ViewComponent("AddServiceRequest");
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddServiceResquests(AddServiceRequestVM addServiceRequestVM)
        {
            ServiceRequest addThisServiceRequest = new()
            {
               

            };
            dcx.ServiceRequests.Add(addThisServiceRequest);
            await dcx.SaveChangesAsync();
            return ViewComponent("ViewServiceRequests");
        }
        public IActionResult EditServiceRequest(string Id)
        {
            return ViewComponent("EditMemo", Id);
        }
        [HttpPost]
        public async Task<IActionResult> EditServiceRequest(string Id, EditServiceRequestVM editServiceRequestVM)
        {

            ServiceRequest serviceRequestToComment = new();
            serviceRequestToComment = (from s in dcx.ServiceRequests where s.ServiceRequestId == Id select s).FirstOrDefault();

            ServiceRequestComment addThisComment = new()
            {
                //MemoId = memoToUpdate.MemoId,
                CreatedDate = DateTime.UtcNow,




                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                //UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.ServiceRequestComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }


    }
}
