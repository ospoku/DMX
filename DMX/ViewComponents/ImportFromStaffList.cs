using CIS.Data;
using CIS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMX.ViewComponents
{
    public class ImportFromStaffList(XContext context) : ViewComponent
    {
        public readonly XContext dcx = context;

        public IViewComponentResult Invoke()
        {
            ImportFromStaffListVM fromStaffListVM = new ImportFromStaffListVM()
            {
                StaffList = new SelectList(dcx.StaffList.ToList(), "StaffId", "Name"),

            };


            return View(fromStaffListVM);
        }
    }
}
