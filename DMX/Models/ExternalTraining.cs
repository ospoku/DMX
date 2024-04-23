using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class ExternalTraining
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TrainingId { get; set; }
        public string Attendee { get; set; }

        public string WorkshopTitle { get; set; }

        public int NumberofDays { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime ProposedTrainingDate { get; set; }
        public string ProposedTrainingGroup { get; set; }
        public string Description { get; set; }
    }
}
