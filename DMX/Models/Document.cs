using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class Document : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DocumentId { get; set; }
        public string DocumentSource { get; set; }
        public DateTime DateReceived { get; set; }
        public DateTime DocumentDate { get; set; }
        public string ReferenceNumber { get; set; }

        public string AdditionalNotes { get; set; }
        [Required]
        public byte[] PDF { get; set; }
        public ICollection<AppUser> ApplicationUsers { get; set; }
        public ICollection<Comment> Comments { get; set; }
  

    }
}
