using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class Permission:TableAudit
    {
        [Key]
        public int PermissionId { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();
        public Module Module { get; set; }
        public int ModuleId { get; set; }
        public ActionItem Action { get; set; }
        public int ActionId { get; set; }
        public string Code { get; set; }
        public void GenerateCode()
        {
            Code = $"{Module?.Name}.{Action?.Name}";
        }
        public string Description { get; set; }
    }
}
