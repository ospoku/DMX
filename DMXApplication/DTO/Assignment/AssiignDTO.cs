using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.DTO.Assignment
{
   public class AssignmentDTO
    {
        public int ModuleId { get; set; }

        public int ModuleRecordId { get; set; }

        public string AssignedTo { get; set; }

        public string? Remarks { get; set; }
    }
}
