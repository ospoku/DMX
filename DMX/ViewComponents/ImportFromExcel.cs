using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMX.ViewComponents
{
    public class ImportFromExcel(XContext context) : ViewComponent
    {
        public readonly XContext dcx = context;

        public IViewComponentResult Invoke()
        {
            ImportFromExcelVM fromExcelVM = new()
            {


            };


            return View(fromExcelVM);
        }
    }
}
