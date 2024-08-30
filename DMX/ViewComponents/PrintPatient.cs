using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class DetailPatient(XContext dContext) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        public IViewComponentResult Invoke(string Id)
        {
            var deceased = dcx.Patients.Where(a => a.IsDeleted == false & a.PatientId == Id).Select(a => a).FirstOrDefault();


            int numberOfDays = (DateTime.Now- deceased.CreatedDate.Value).Days;
            decimal totalFee = 0;
            foreach (var day in dcx.FeeStructures.Select(f => f).ToList())
            {

                if (numberOfDays > day.MaxDays)
                {
                    totalFee += (day.MaxDays - day.MinDays + 1) * day.Fee;
                }
                else if (numberOfDays >= day.MinDays)
                {
                    totalFee += (numberOfDays - day.MinDays + 1) * day.Fee;
                    break;

                }

            }

                return View(deceased);
            }
        }
    }

