using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using DMX.DataProtection;
using Microsoft.EntityFrameworkCore;
using DMX.Controllers;

namespace DMX.ViewComponents
{
    public class PrintPatient(XContext dContext, PatientController settings) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly PatientController pc = settings;

        public IViewComponentResult Invoke(string Id)
        {
            var deceased = dcx.Patients.Include(d => d.PatientComments.OrderBy(d => d.CreatedDate)).Where(d => d.IsDeleted == false & d.PatientId == @Encryption.Decrypt(Id)).Select(d => d)
            .FirstOrDefault();


            int numberOfDays = (DateTime.Now - deceased.CreatedDate.Value).Days;
            var calculatedFee = pc.FeecalCalculator(numberOfDays);
            PrintMorgueVM printMorgueVM = new()
            {
                FinalDiagnoses = deceased.FinalDiagnoses,
                FolderNo = deceased.FolderNo,
                Date = deceased.Date,
                DeceasedTypeId = deceased.DeceasedTypeId,
                DepositorAddress = deceased.DepositorAddress,
                Depositor = deceased.Depositor,
                Description = deceased.Description,
                TagNo = deceased.TagNo,
                WardInCharge = deceased.WardInCharge,
                AmountOwing=calculatedFee
            };


           

                return View(printMorgueVM);
            }
        }
    }

