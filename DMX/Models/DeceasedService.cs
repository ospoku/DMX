using DMX.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class DeceasedService : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string MorgueServiceId { get; set; }
        [ForeignKey(nameof(MorgueServiceId))]
        public MorgueService MorgueService { get; set; }

        // Foreign key to Deceased
        public  string DeceasedId { get; set; }

        // Navigation property to Deceased
        [ForeignKey(nameof(DeceasedId))]
        public  Deceased Deceased { get; set; }
    }
    }

