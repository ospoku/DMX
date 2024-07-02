﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class EditProfileVM
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string BranchId { get; set; }
        public SelectList Branches { get; set; }
        public string ApplicationRoleId { get; set; }
        public List<SelectListItem> ApplicationRoles { get; set; }

    }
}
