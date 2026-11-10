using DMXApplication.DTO;
using DMXApplication.DTO.Memos;
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
        Task<int> Assign(AssignMemoDTO assignment);
    }
}
