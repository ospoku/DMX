using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;
using DMX.ViewComponents;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DMX.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly XContext _context;
        private readonly INotyfService _notyfService;
        private readonly EntityService _entityService;
        private readonly AssignmentService _assignmentService;
        public ServiceRequestController(
            XContext context,
            UserManager<AppUser> userManager,
            INotyfService notyfService,
            EntityService entityService,
            AssignmentService assignmentService)
        {
            _userManager = userManager;
            _context = context;
            _notyfService = notyfService;
            _entityService = entityService;
            _assignmentService = assignmentService;
            
        }

        [HttpGet]
        public IActionResult ViewServiceRequests() => ViewComponent(nameof(ViewServiceRequests));

        [HttpGet]
        public IActionResult AddServiceRequest() => ViewComponent(nameof(AddServiceRequest));

        [HttpPost]
        public async Task<IActionResult> AddServiceRequest(AddServiceRequestVM addServiceRequestVm, IFormFile formFile)
        {
            try
            {
                var newServiceRequest = new ServiceRequest
                {
                    Description = addServiceRequestVm.Description,
                    RequestTypeId = addServiceRequestVm.RequestTypeId,
                    CategoryId = addServiceRequestVm.CategoryId,
                    StatusId = addServiceRequestVm.StatusId,
                    PriorityId = addServiceRequestVm.PriorityId,
                    Title = addServiceRequestVm.Title,

                };

                if (formFile != null)
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);
                        newServiceRequest.Attachments = memoryStream.ToArray();
                    }
                }

                bool result = await _entityService.AddEntityAsync(newServiceRequest, User);
                if (!result)
                {
                    _notyfService.Error("Failed to add service request. Please try again.", 5);
                    return ViewComponent(nameof(ViewServiceRequests));
                }
            
                foreach (var userId in addServiceRequestVm.SelectedUsers)
                {
                    var assignment = new ServiceAssignment
                    {
                        ServiceRequestId = newServiceRequest.RequestId,
                        UserId = userId
                    };

                    bool assignResult = await _assignmentService.AssignUsers(assignment, User);
                    if (!assignResult)
                    {
                        _notyfService.Error($"Failed to assign request to user {userId}.", 5);
                    }
                }
                _notyfService.Success("Service Request and assignments successfully processed.", 5);
                return RedirectToAction(nameof(ViewServiceRequests));
            }
            catch (Exception ex)
            {
                _notyfService.Error("An error occurred while processing the request.", 5);
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request." });
            }
        }
        

        [HttpPost]
        public async Task<IActionResult> CommentServiceRequest(string Id, ServiceRequestCommentVM commentVm)
        {
            try
            {
                var serviceRequest = await _context.ServiceRequests.FirstOrDefaultAsync(s => s.RequestId == Id);
                if (serviceRequest == null)
                {
                    return NotFound();
                }

                var newComment = new ServiceRequestComment
                {
                    ServiceRequestId = serviceRequest.RequestId,
                    Message = commentVm.NewComment,
                    UserId = (await _userManager.GetUserAsync(User)).Id
                };

                bool result = await _entityService.AddEntityAsync(newComment, User);
                if (result)
                {
                    _notyfService.Success("Comment successfully saved", 5);
                }
                else
                {
                    _notyfService.Error("Comment could not be saved!!!", 5);
                }

                return RedirectToAction(nameof(ViewServiceRequests));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request.", ex.Message });
            }
        }


        [HttpGet]
        public IActionResult EditServiceRequest(string Id) => ViewComponent(nameof(EditServiceRequest), Id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditServiceRequest(string id, EditServiceRequestVM editServiceRequestVm)
        {
            if (editServiceRequestVm.SelectedUsers == null || !editServiceRequestVm.SelectedUsers.Any())
            {
                _notyfService.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewServiceRequests");
            }

            try
            {
                var decryptedId = Encryption.Decrypt(id);
                var serviceRequestToUpdate = await _context.ServiceRequests.FirstOrDefaultAsync(s => s.RequestId == decryptedId);
                if (serviceRequestToUpdate == null)
                {
                    _notyfService.Error("Service request not found.", 5);
                    return RedirectToAction("ViewServiceRequests");
                }

                // Update service request properties


                bool isEdited = await _entityService.EditEntityAsync(serviceRequestToUpdate, User);
                if (!isEdited)
                {
                    _notyfService.Error("Failed to update service request. Please try again.", 5);
                    return RedirectToAction("ViewServiceRequests");
                }

                // Remove existing assignments
                var existingAssignments = _context.ServiceAssignments.Where(x => x.ServiceRequestId == decryptedId);
                _context.ServiceAssignments.RemoveRange(existingAssignments);

                // Add new assignments
                var newAssignments = editServiceRequestVm.SelectedUsers
                    .Select(userId => new ServiceAssignment { ServiceRequestId = decryptedId, UserId = userId })
                    .ToList();

                await _context.ServiceAssignments.AddRangeAsync(newAssignments);
                await _context.SaveChangesAsync();

                _notyfService.Success("Service request successfully updated.", 5);
                return RedirectToAction("ViewServiceRequests");
            }
            catch (Exception ex)
            {
                _notyfService.Error("An unexpected error occurred. Please try again.", 5);
                Console.WriteLine($"Error updating service request: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the service request." });
            }
        }

        [HttpGet]
        public IActionResult CommentServiceRequest(string Id) => ViewComponent(nameof(CommentServiceRequest), Id);

    }
}