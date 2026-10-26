using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMXDomain.Models
{
    public class SickReport:TableAudit
    {[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SickReportId { get; set; }
        public string AdditionalNotes { get; set; }
    }
}
