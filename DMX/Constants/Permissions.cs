using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMX.Constants
{
    public class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
           {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
            };
        }

        public static class Modules
        {
            public const string ViewLetters = "View Letters";
            public const string CreateLetter = "Permissions.Products.Create";
            public const string EditLetter = "Permissions.Products.Edit";
            public const string DeleteLetter = "Permissions.Products.Delete";
            public const string ViewMemo = "Permissions.Products.View";
            public const string CreateMemo = "Permissions.Products.Create";
            public const string EditMemo = "Permissions.Products.Edit";
            public const string DeleteMemo = "Permissions.Memo.Delete";
        }
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string ManageRoles = "Permissions.Users.ManageRoles";
            public const string ManageClaims = "Permissions.Users.ManageClaims";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string ManageClaims = "Permissions.Roles.ManageClaims";

        }
    }
}