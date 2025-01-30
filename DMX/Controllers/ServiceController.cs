using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.Controllers
{
    public class ServiceController( XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService, EntityService entityService
           ) : Controller
    {
        public readonly UserManager<AppUser> usm = userManager;
    public readonly XContext dcx = dContext;
    private readonly INotyfService notyf = notyfService;
        public readonly EntityService entityServ = entityService;
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
            if (editServiceRequestVM.SelectedUsers == null || !editServiceRequestVM.SelectedUsers.Any())
            {
                notyf.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewServiceRequests");
            }

            try
            {
                var decryptedId = Encryption.Decrypt(Id);
                var updateThisServiceRequest = await dcx.ServiceRequests.FirstOrDefaultAsync(s => s.ServiceRequestId == decryptedId);

                if (updateThisServiceRequest == null)
                {
                    notyf.Error("Service request not found.", 5);
                    return RedirectToAction("ViewServiceRequests");
                }

                // Update Service Request properties
                updateThisServiceRequest.ActionToBeTaken = editServiceRequestVM.ActionToBeTaken;
                updateThisServiceRequest.FaultInspectedBy = editServiceRequestVM.FaultInspectedBy;
                updateThisServiceRequest.Faults = editServiceRequestVM.Faults;

                bool IsEdited = await entityServ.EditEntityAsync(updateThisServiceRequest, User);

                if (!IsEdited)
                {
                    notyf.Error("Failed to update service request. Please try again.", 5);
                    return RedirectToAction("ViewServiceRequests");
                }

                // Remove existing assignments
                var existingAssignments = dcx.ServiceAssignments.Where(x => x.ServiceRequestId == decryptedId);
                dcx.ServiceAssignments.RemoveRange(existingAssignments);
                await dcx.SaveChangesAsync();

                // Add new assignments
                var newAssignments = editServiceRequestVM.SelectedUsers
                    .Select(userId => new ServiceAssignment { ServiceRequestId = decryptedId, UserId = userId })
                    .ToList();

                await dcx.ServiceAssignments.AddRangeAsync(newAssignments);
                await dcx.SaveChangesAsync();

                notyf.Success("Service request successfully updated.", 5);
                return RedirectToAction("ViewServiceRequests");
            }
            catch (Exception ex)
            {
                notyf.Error("An unexpected error occurred. Please try again.", 5);
                Console.WriteLine($"Error updating service request: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the service request." });
            }
        }

    }
}
