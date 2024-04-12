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
            var documentDetail = dcx.Patients.Where(a => a.IsDeleted == false & a.PatientId == Id).Select(a => new DetailDocumentVM

       

           
            {
          
                
            }).FirstOrDefault();

            return View(documentDetail);
        }
            
    }
}
