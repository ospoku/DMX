using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class ViewPerDiems(XContext context):ViewComponent
    {
        public readonly XContext ctx=context;
        public IViewComponentResult Invoke()
        {

            var perDiems=ctx.PerDiems.Select(p=>new ViewPerDiemsVM
            {

                Username=p.UserId,  
                Amount=p.Amount,
                Department=p.Department.Name,
                
                Rank=p.Rank,
            }).ToList();

            return View(perDiems);
        }
    }
}
