using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using DMX.Services;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
namespace DMX.ViewComponents
{
    public class ViewPatients(XContext dContext, FeeService feeService) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly FeeService fs = feeService;

        public IViewComponentResult Invoke()
        {
            var patients = dcx.Patients
                .Where(a => !a.IsDeleted)
                .OrderByDescending(t => t.CreatedDate)
                .ToList();

            var pList = patients.Select(a =>
            {

                TimeSpan timeSpan = DateTime.Now - a.CreatedDate.Value;
                int numberofDays = (int)timeSpan.TotalDays;
                return new ViewPatientsVM
                {
                    PatientId = a.PatientId,
                    PatientName = a.Name,
                    Date = a.Date,
                    FinalDiagnoses = a.Diagnoses,
                    FolderNo = a.FolderNo,
                    WardInCharge = a.WardInCharge,
                    OtherFees = fs.FeecalCalculator(numberofDays),
                    TagNo = a.TagNo,
                    CreatedDate = a.CreatedDate
                };
            }).ToList();
            return View(pList);
        }
    }
}
