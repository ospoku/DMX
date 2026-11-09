using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DMX.ViewModels
{
    public class AddMemoVM
    {
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please select assignees")]
        public List<string> SelectedUsers { get; set; } = [];

    
        public SelectList? UsersList { get; set; } 
        [Required]
        public string Title { get; set; } = string.Empty;
    
    }
}
