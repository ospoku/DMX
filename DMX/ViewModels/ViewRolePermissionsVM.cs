namespace DMX.ViewModels
{
    public class ViewRolePermissionsVM   
    { public string PublicId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
   
        public string PublicId { get; set; }
        public string ModuleName { get; set; }
       public string ActionName {get;set; }
        public List<string> PermissionCodes { get; set; }
        public List<bool> RoleClaims { get; set; }


    }


}