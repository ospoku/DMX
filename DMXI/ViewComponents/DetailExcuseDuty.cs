﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using DMX.DataProtection;

namespace DMX.ViewComponents
{
    public class DetailExcuseDuty(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)
        {
            

            ExcuseDuty excuseDutyDetail = new();
            excuseDutyDetail = (from a in dcx.ExcuseDuties where a.Id == @Encryption.Decrypt(Id) & a.IsDeleted == false select a).FirstOrDefault();
            DetailExcuseDutyVM excuseDutyVM = new ()
            {
                Date = new ExcuseDuty().Date,
                DateofDischarge=new ExcuseDuty().DateofDischarge,
                ExcuseDays=new ExcuseDuty().ExcuseDays,
             
                OperationDiagnosis = new ExcuseDuty().OperationDiagnosis,
               SelectedUsers = (from x in dcx.ExcuseDutyAssignments where x.ExcuseDutyId == @Encryption.Decrypt(Id) select x.AppUserId).ToList(),
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };
            return View(excuseDutyVM);
        }
    }
}