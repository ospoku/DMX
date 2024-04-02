﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DMX.ViewModels
{
    public class EditDocumentVM
    {

        public string ReferenceNumber { get; set; }
        public string DocumentSource { get; set; }
        public DateTime DateReceived { get; set; }
        public DateTime DocumentDate { get; set; }
        [FileExtensions(Extensions = "jpg,jpeg,png,pdf")]
        public IFormFile UploadFile { get; set; }
        public string AdditionalNotes { get; set; }
        public string[] SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }



    }
}
