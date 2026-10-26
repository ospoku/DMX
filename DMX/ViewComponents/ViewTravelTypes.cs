using Microsoft.AspNetCore.Mvc;
using DMX.ViewModels;
using DMXInfrastructure.Data;

namespace DMX.ViewComponents
{
    public class ViewTravelTypes(XContext dContext) : ViewComponent
    {
        public readonly XContext dcx = dContext;
       

        public IViewComponentResult Invoke()
        {
          
            var tList = dcx.TravelTypes.Where(a => a.IsDeleted == false).Select(a => new ViewTravelTypesVM
            {
                Id = a.TravelTypeId,
                Name = a.Name,
               Code =a.Code,
               Description = a.Description,
            
            }).ToList();
            return View(tList);
        }
    }
}
