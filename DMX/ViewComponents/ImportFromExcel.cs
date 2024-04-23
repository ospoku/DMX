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
    public class ImportFromExcel : ViewComponent
    {
        public readonly XContext dcx;
        public ImportFromExcel(CISContext context)
        {
            dcx = context;
        }

        public IViewComponentResult Invoke()
        {
            ImportFromExcelVM fromExcelVM = new ImportFromExcelVM()
            {


            };


            return View(fromExcelVM);
        }
    }
}
