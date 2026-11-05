using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.DTO.Letters
{
    public class AddLetterDTO
    {
        public string Source { get; internal set; }
        public string Subject { get; internal set; }
        public DateTime ReceiptDate { get; internal set; }
        public DateTime DocumentDate { get; internal set; }
        public string AdditionalNotes { get; internal set; }
    }
}
