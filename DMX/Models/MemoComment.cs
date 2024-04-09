﻿using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class MemoComment : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Message { get; set; }
        public string MemoId {  get; set; } 
        public string UserId { get; set; }
        public Memo Memo { get; set; }
        public AppUser AppUser { get; set; }
    }
}
