using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class ViewTransportAllowances(XContext context):ViewComponent

    {
        public readonly XContext ctx = context;
        public IViewComponentResult Invoke()
        {
            var allowance = ctx.TransportAllowances.Select(x=>new ViewTransportAllowancesVM {
            
            Id=x.Id,
           
           Amount = x.Allowance,

            }) .ToList(); 

            return View(allowance);
        }
    }
}
