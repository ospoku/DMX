using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewWorkshops(XContext dContext) : ViewComponent
    {
        public readonly XContext dcx = dContext;
       

        public IViewComponentResult Invoke()
        {


            var workshopList = dcx.Workshops.Where(d => d.IsDeleted == false).Select(d => new ViewWorkshopsVM
            {

                WorkshopId = d.WorkshopId,
               Description = d.Description,
               Name = d.Name,
       
               
               

            }).ToList();

           
            return View(workshopList);
        }
    }
}
