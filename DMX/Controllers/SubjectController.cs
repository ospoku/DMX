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
    public class SubjectController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly XContext _context;
        private readonly INotyfService _notyfService;
        private readonly EntityService _entityService;
        private readonly AssignmentService _assignmentService;
        public SubjectController(
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
        public async Task<IActionResult> AddServiceRequest(AddSubjectVM addSubjectVM)
        {
            try
            {
                var newSubject = new Subject
                {
                    Name = addSubjectVM.Name,
                    Description = addSubjectVM.Description,
                    FacultyId = addSubjectVM.FacultyId,
                    DepartmentId = addSubjectVM.DepartmentId,

                };



                bool result = await _entityService.AddEntityAsync(newSubject, User);
                if (!result)
                {
                    _notyfService.Error("Failed to add service request. Please try again.", 5);
                    return ViewComponent(nameof(ViewServiceRequests));
                }

                else
                {

                    _notyfService.Success("Service Request and assignments successfully processed.", 5);
                    return RedirectToAction(nameof(ViewServiceRequests));
                }
            }
            catch (Exception ex)
            {
                _notyfService.Error("An error occurred while processing the request.", 5);
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request." });
            }




        }

        [HttpGet]
        public IActionResult EditServiceRequest(string Id) => ViewComponent(nameof(EditServiceRequest), Id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditServiceRequest(string id, EditSubjectVM editServiceRequestVm)
        {
            if (editServiceRequestVm.SelectedUsers == null || !editServiceRequestVm.SelectedUsers.Any())
            {
                _notyfService.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewServiceRequests");
            }

            try
            {
                var decryptedId = Encryption.Decrypt(id);
                var serviceRequestToUpdate = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId == decryptedId);
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
                else
                {

                    _notyfService.Success("Service request successfully updated.", 5);
                    return RedirectToAction("ViewServiceRequests");
                }
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