using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class ExcuseDuty : TableAudit
    {[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
       
        public DateTime Date { get; set; }
        public DateTime DateofDischarge { get; set; }
        public string OperationDiagnosis { get; set; }
        public string ExcuseDays { get; set; }

        public string ReferenceNumber { get; set; } = "E" + Guid.NewGuid().ToString("N").Substring(0, 5);
            

    }
}
