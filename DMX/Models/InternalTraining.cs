﻿using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class InternalTraining : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TrainingId { get; set; }
        public string EventName { get; set; }
        public DateTime Date { get; set; }

        public string Description { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}
