using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMXDomain.Models
{
    public class DeceasedService : TableAudit
    {
        [Key]
        public int Id { get; set; }
        public Guid PublicId { get; set; }= Guid.NewGuid();

        public int MorgueServiceId { get; set; }
        [ForeignKey(nameof(MorgueService.Id))]
        public MorgueService MorgueService { get; set; }

        // Foreign key to Deceased
        public  int DeceasedId { get; set; }

        // Navigation property to Deceased
        [ForeignKey(nameof(DeceasedId))]
        public  Deceased Deceased { get; set; }
    }
    }

