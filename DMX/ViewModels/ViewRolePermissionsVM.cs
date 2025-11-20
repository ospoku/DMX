namespace DMX.ViewModels
{
    public class ViewRolePermissionsVM   
    {
        public Guid PublicId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
   
      
        public string Module { get; set; }
       public string Action {get;set; }
        public List<string> PermissionCodes { get; set; }
        public List<string> SelectedPermissions { get; set; }
      
    



    }


}