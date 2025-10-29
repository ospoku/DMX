namespace DMX.ViewModels
{
    public class ViewPermissionsVM
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string> AssignedPermissions { get; set; }
    }
}
