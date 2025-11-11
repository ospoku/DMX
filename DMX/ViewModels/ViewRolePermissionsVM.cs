namespace DMX.ViewModels
{
    public class ViewRolePermissionsVM   
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string>RoleClaims { get; set; }
    }

    //public class UserRolesVM
    //{
    //    public string Id { get; set; }
    //    public string RoleName { get; set; }
    //    public bool Selected { get; set; }
    //}
}