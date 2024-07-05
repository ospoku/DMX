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
            public const string ViewLetters = "Permissions.Modules.ViewLetters";
            public const string CreateLetter = "Permissions.Modules.CreateLetter";
            public const string EditLetter = "Permissions.Modules.EditLetter";
            public const string DeleteLetter= "Permissions.Modules.DeleteLetter";
            public const string ViewMemo = "Permissions.Modules.ViewMemo";
            public const string CreateMemo = "Permissions.Modules.CreateMemo";
            public const string EditMemo = "Permissions.Modules.EditMemo";
            public const string DeleteMemo = "Permissions.Modules.DeleteMemo";
        }
        public static class User
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