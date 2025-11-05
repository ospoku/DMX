using System.ComponentModel.DataAnnotations;

namespace DMX.Models
{
    public class ActionItem
    {
        [Key]
        public int ActionId { get; set; }
        public string Name { get; set; }
    }
}
