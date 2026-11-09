using DMXApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
    public interface IAssignmentService
    {
        Task<bool> AddAssignment(AssignDTO assignDTO);
    }
}
