﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class AddDeceasedVM
    {
        public string DeceasedId { get; set; }
        public string ReferenceNumber { get; set; }
        public string Deceased { get; set; }
        public string Depositor { get; set; }
        public string DepositorAddress { get; set; }
        public string Diagnoses { get; set; }
        public string WardInCharge { get; set; }
        public DateTime Date { get; set; }
        public string TagNo { get; set; }
        public string FolderNo {  get; set; }
        public string DeceasedTypeId { get; set; }
        public SelectList DeceasedTypes { get; set; }
        public string Description { get; set; }
        public List<string> SelectedUsers { get; set; }
  
        public SelectList UsersList { get; set; }
    }
}
