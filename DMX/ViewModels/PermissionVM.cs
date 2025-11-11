using DMX.Models;
using System.Collections.Generic;

namespace DMX.ViewModels
{
    public class PermissionVM
    {
        public int PermissionId { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();
        public Module Module { get; set; }
        public int ModuleId { get; set; }
        public ActionItem Action { get; set; }
        public int ActionId { get; set; }
        public string Code { get; set; }

        // Add this property for UI selection
        public bool Selected { get; set; } = false;
    }

}
