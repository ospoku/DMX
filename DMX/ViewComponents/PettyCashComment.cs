﻿using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class PettyCashComment(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)


        {
            var AssignedUsers= new List<string>();

            //var result = from u in dcx.Assignments where u.MemoId == Id select u.ApplicationUser.Id;
            //foreach (var user in result) {
            //    AssignedUsers.Add(user);
            //}

            PettyCash pettyCashToEdit = new PettyCash();
            pettyCashToEdit = (from m in dcx.PettyCashes.Include(m => m.Comments.OrderBy(m => m.CreatedDate)) where m.PettyCashId == Id select m).FirstOrDefault();

            PettyCashCommentVM addCommentVM = new PettyCashCommentVM
            {
               
                Comments = pettyCashToEdit.Comments,
                
                SelectedUsers = AssignedUsers,

        
                UsersList= new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };
            

            return View(addCommentVM);
        }
    }
}