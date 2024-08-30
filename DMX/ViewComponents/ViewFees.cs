using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class ViewFees(XContext context):ViewComponent
    {
        public readonly XContext ctx=context;
        public IViewComponentResult Invoke()
        { FeesVM feesVM = new FeesVM();
            
            //feesVM.PCThreshold = (decimal)ctx.Fees.Select(x=>x.PCThreshold).FirstOrDefault();
            //feesVM.Tier1=(decimal)ctx.Fees.Select((x)=>x.Tier1).FirstOrDefault();   
            //feesVM.Tier2= (decimal)ctx.Fees.Select((x)=>x.Tier2).FirstOrDefault();


            return View(feesVM);
        }

    }
}
