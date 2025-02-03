using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;
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

        public ServiceRequestController(
            XContext context,
            UserManager<AppUser> userManager,
            INotyfService notyfService,
            EntityService entityService)
        {
            _userManager = userManager;
            _context = context;
            _notyfService = notyfService;
            _entityService = entityService;
        }

        [HttpGet]
        public IActionResult ViewServiceRequests() => ViewComponent(nameof(ViewServiceRequests));

        [HttpGet]
        public IActionResult AddServiceRequest() => ViewComponent(nameof(AddServiceRequest));

        [HttpPost]
        public async Task<IActionResult> AddServiceRequest(AddServiceRequestVM addServiceRequestVm)
        {
            try
            {
                var newServiceRequest = new ServiceRequest
                {
                    ActionToBeTaken = addServiceRequestVm.ActionToBeTaken,
                  
                    Faults = addServiceRequestVm.Faults
                };

                bool result = await _entityService.AddEntityAsync(newServiceRequest, User);
                if (result)
                {
                    _notyfService.Success("Service request successfully added.", 5);
                    return RedirectToAction("ViewServiceRequests");
                }
                else
                {
                    _notyfService.Error("Failed to add service request. Please try again.", 5);
                    return ViewComponent("AddServiceRequest");
                }
            }
            catch (Exception ex)
            {
                _notyfService.Error("An error occurred while processing the request.", 5);
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ServiceRequestComment(string id, MemoCommentVM commentVm)
        {
            try
            {
                var serviceRequest = await _context.ServiceRequests.FirstOrDefaultAsync(s => s.ServiceRequestId == id);
                if (serviceRequest == null)
                {
                    return NotFound();
                }

                var newComment = new ServiceRequestComment
                {
                    ServiceRequestId = serviceRequest.ServiceRequestId,
                    Message = commentVm.NewComment,
                    CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name")?.Value,
                    CreatedDate = DateTime.Now
                };

                _context.ServiceRequestComments.Add(newComment);
                await _context.SaveChangesAsync();

                _notyfService.Success("Comment successfully added.", 5);
                return RedirectToAction("ViewServiceRequests");
            }
            catch (Exception ex)
            {
                _notyfService.Error("An error occurred while adding the comment.", 5);
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the comment." });
            }
        }

        [HttpGet]
        public IActionResult EditServiceRequest(string id) => ViewComponent("EditServiceRequest", id);

        [HttpPost]
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
                var serviceRequestToUpdate = await _context.ServiceRequests.FirstOrDefaultAsync(s => s.ServiceRequestId == decryptedId);
                if (serviceRequestToUpdate == null)
                {
                    _notyfService.Error("Service request not found.", 5);
                    return RedirectToAction("ViewServiceRequests");
                }

                // Update service request properties
                serviceRequestToUpdate.ActionToBeTaken = editServiceRequestVm.ActionToBeTaken;
               
                serviceRequestToUpdate.Faults = editServiceRequestVm.Faults;

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
    }
}