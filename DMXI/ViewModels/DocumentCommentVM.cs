﻿using DMX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class DocumentCommentVM
    {
        public string MemoId { get; set; }
        public string MemoContent { get; set; }
        public string Title { get; set; }

        public string NewComment { get; set; }
    public ICollection<LetterComment> Comments { get; set; }  
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
    }
}