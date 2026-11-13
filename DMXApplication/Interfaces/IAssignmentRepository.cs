using DMXApplication.DTO;
using DMXApplication.DTO.Assignment;
using DMXDomain.Models;
using System.Linq.Expressions;

namespace DMXApplication.Interfaces
{
    public interface IAssignmentRepository
    {
        public Task<int> Assign(Assignment assignment);
public Task<int> Update(Assignment assignment);


    }
}