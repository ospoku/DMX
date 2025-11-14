namespace DMX.ViewModels
{
    public class ViewRolePermissionsVM   
    { public string PublicId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string>RoleClaims { get; set; }

       public string Module { get; set; }
       public string Action {get;set; }


    }


}