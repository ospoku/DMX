﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DMX.ViewModels
{
    public class AddFeeStructureVM
    {
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public string DeceasedTypeId{get;set;}
        public SelectList DeceasedTypes { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        
        public decimal Fee { get; set; }
    }
}
