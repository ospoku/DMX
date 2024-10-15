using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class ViewPettyCashLimits(XContext context):ViewComponent
    {
        public readonly XContext ctx=context;
        public IViewComponentResult Invoke()
        {

            var pettyCashlimits=ctx.PettyCashLimits.Select(p=>new ViewPettyCashLimitVM
            {

              
                Amount=p.PettyCashLimitAmount,
          
            }).ToList();

            return View(pettyCashlimits);
        }
    }
}
