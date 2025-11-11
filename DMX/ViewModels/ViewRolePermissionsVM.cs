namespace DMX.ViewModels
{
    public class ViewRolePermissionsVM   
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string>RoleClaims { get; set; }

       public string ModuleName { get; set; }
       public string ActionName {get;set; }


    }


}