using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class EditExcuseDuty:ViewComponent
    {
        public readonly XContext dcx;
        public EditExcuseDuty(XContext dContext)
        {
            dcx = dContext;
        }

        public IViewComponentResult Invoke(string ExcuseId)


        {
           

           ExcuseDuty dutyToUpdate = new ExcuseDuty();
            dutyToUpdate = (from d in dcx.ExcuseDuties where d.ExcuseFormId==ExcuseId select d ).FirstOrDefault();

            EditExcuseDutyVM editMemoVM = new EditExcuseDutyVM
            {
               

            };
            

            return View(editMemoVM);
        }
    }
}
