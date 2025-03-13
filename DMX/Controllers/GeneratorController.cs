using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMX.Controllers
{
    [Authorize]
    public class GeneratorController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly XContext _context;
        private readonly INotyfService _notyfService;
        private readonly EntityService _entityService;
        private readonly TimetableGenerator _timetableGenerator;

        public GeneratorController(
            XContext context,
            UserManager<AppUser> userManager,
            INotyfService notyfService,
            EntityService entityService,
            TimetableGenerator timetableGenerator)
        {
            _userManager = userManager;
            _context = context;
            _notyfService = notyfService;
            _entityService = entityService;
            _timetableGenerator = timetableGenerator;
        }

        [HttpGet]
        public IActionResult GenerateTimetable()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateTimetable(GenerateTimetableVM model)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Invalid input. Please check the form and try again.", 5);
                return View(model);
            }

            try
            {
                // Generate the timetable
                var timetable = _timetableGenerator.GenerateTimetable();

                // Save the generated timetable to the database
                _context.TimetableEntries.AddRange(timetable);
                await _context.SaveChangesAsync();

                _notyfService.Success("Timetable generated successfully!", 5);
                return RedirectToAction(nameof(ViewTimetable));
            }
            catch (Exception ex)
            {
                _notyfService.Error($"An error occurred: {ex.Message}", 5);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ViewTimetable()
        {
            var timetable = _context.TimetableEntries
                .Include(te => te.Group)
                .Include(te => te.Teacher)
                .Include(te => te.Classroom)
                .Include(te => te.TimeSlot)
                .ToList();

            return View(timetable);
        }
    }
}