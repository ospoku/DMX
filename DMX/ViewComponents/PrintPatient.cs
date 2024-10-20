using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using DMX.DataProtection;
using Microsoft.EntityFrameworkCore;
using DMX.Controllers;
using DMX.Services;

namespace DMX.ViewComponents
{
    public class PrintPatient(XContext dContext, FeeService feeService) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly FeeService fs = feeService;

        public IViewComponentResult Invoke(string Id)
        {
            var deceased = dcx.Patients.Include(d => d.PatientComments.OrderBy(d => d.CreatedDate)).Where(d => d.IsDeleted == false & d.PatientId == @Encryption.Decrypt(Id)).Select(d => d)
            .FirstOrDefault();
        
            TimeSpan difference = DateTime.Now - deceased.CreatedDate.Value;
            int numberOfDays = (int)difference.TotalDays;

            PrintMorgueVM printMorgueVM = new()
            {
                FinalDiagnoses = deceased.Diagnoses,
                FolderNo = deceased.FolderNo,
                Date = deceased.Date,
                DeceasedTypeId = deceased.DeceasedTypeId,
                DepositorAddress = deceased.DepositorAddress,
                Depositor = deceased.Depositor,
                Description = deceased.Description,
                TagNo = deceased.TagNo,
                WardInCharge = deceased.WardInCharge,
                AccruedFees= fs.FeecalCalculator(numberOfDays),
            };


           

                return View(printMorgueVM);
            }
        }
    }

