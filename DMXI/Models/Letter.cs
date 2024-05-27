using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class Letter : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string LetterId { get; set; }
        public  string Source { get; set; }
        public DateTime DateReceived { get; set; }
        public DateTime DocumentDate { get; set; }
        public  string ReferenceNumber { get; set; }

        public string AdditionalNotes { get; set; }
        [Required]
        public  byte[] PDF { get; set; }
        
  

    }
}
