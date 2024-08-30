﻿using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class ViewFeeStructures(XContext context):ViewComponent
    {
        public readonly XContext ctx=context;
        public IViewComponentResult Invoke()
        {
            var lList = ctx.FeeStructures.Where(f => f.IsDeleted == false).Select(t => new ViewFeeStructuresVM 
            {
               Fee=t.Fee,
               Max=t.MaxDays,
               Min=t.MinDays,
               Id=t.Id  ,
            }).ToList();
            
    


            return View(lList);
        }

    }
}
