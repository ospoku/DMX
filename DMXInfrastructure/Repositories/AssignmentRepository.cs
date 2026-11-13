using DMXApplication.DTO;
using DMXApplication.Interfaces;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DMXInfrastructure.Repositories
{
    public class AssignmentRepository(XContext context)
        : IAssignmentRepository
    {
        private readonly XContext dcx = context;

       public  async Task<int> Assign(Assignment assignment)
        {

            return dcx.Assignments.Add(assignment);
        }
     
      

    }
}