using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.DTO
{
  public class AssignDTO
    {  public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid PublicId { get; internal set; }

        public AssignDTO() { }
    }
}
