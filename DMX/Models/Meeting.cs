﻿using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class Meeting:TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MeetingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set;}
        public DateTime Date { get; set; }  

    }
}
