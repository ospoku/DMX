﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class Memo : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MemoId { get; set; }
        public string ReferenceId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

      
        public ICollection<MemoComment> MemoComments { get; set; }
    
      
    }
}
