using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DMX.ViewModels
{
    public class AddMemoVM
    {
        [Required]
        public string Content { get; set; }

        public string[] SelectedUsers { get; set; }
        [Required(ErrorMessage = "Please select assignees")]

        public SelectList UsersList { get; set; }
        [Required]
        public string Title { get; set; }
        public string To { get; set; }
        public string From { get; set; }
    }
}
