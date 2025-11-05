using System.ComponentModel.DataAnnotations;

namespace DMX.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }
        public string Name { get; set; }
    }
}
