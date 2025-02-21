﻿using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddDeceased(UserManager<AppUser> userManager, XContext xContext) : ViewComponent
    {
        
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = xContext;
        public IViewComponentResult Invoke()
        {
            AddDeceasedVM addDeceasedVM = new()
            {
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),
                DeceasedTypes = new SelectList(dcx.DeceasedTypes.ToList(), "DeceasedTypeId", "Code"),
                MorgueServices = dcx.MorgueServices.Select(d => new CheckBoxItem
                {
                    Id = d.MorgueServiceId, // Map the Id property
                    Name = d.ServiceName, // Map the Name property
                    IsChecked = false, // Default value for IsSelected}).ToList()
               Amount=d.Amount }).ToList(),
            };
            return View(addDeceasedVM);
        }
    }
}
