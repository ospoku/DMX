using DMXApplication.DTO;
using DMXApplication.DTO.Assignment;
using DMXDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
    public interface IAssignmentService
    {
        Task<int> Assign(AssignDTO assignment);
        Task<int> Update(AssignDTO assignment);
        Task<int> Delete(int id);
    }
}
