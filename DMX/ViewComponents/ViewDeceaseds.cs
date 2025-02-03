using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using DMX.Services;
using System.Linq;

namespace DMX.ViewComponents
{
    public class ViewDeceaseds : ViewComponent
    {
        private readonly XContext _context;
        private readonly FeeService _feeService;

        public ViewDeceaseds(XContext context, FeeService feeService)
        {
            _context = context;
            _feeService = feeService;
        }

        public IViewComponentResult Invoke()
        {
            var deceasedRecords = _context.Deceased
                .Where(a => !a.IsDeleted)
                .OrderByDescending(t => t.CreatedDate)
                .ToList();

            var deceasedList = deceasedRecords.Select(a =>
            {
                TimeSpan timeSpan = DateTime.Now - a.CreatedDate.Value;
                int numberOfDays = (int)timeSpan.TotalDays;

                // Determine if the deceased died in the ward or was brought in dead
                
                return new ViewPatientsVM
                {
                    PatientId = a.DeceasedId,
                    PatientName = a.Name,
                    FinalDiagnoses = a.Diagnoses,
                    FolderNo = a.FolderNo,
                    WardInCharge = a.WardInCharge,
                    OtherFees = _feeService.FeeCalculator(numberOfDays,a.DeceasedTypeId),
                    TagNo = a.TagNo,
                    CreatedDate = a.CreatedDate
                };
            }).ToList();

            return View(deceasedList);
        }
    }
}