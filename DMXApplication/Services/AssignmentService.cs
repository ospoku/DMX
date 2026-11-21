using DMXApplication.DTO;
using DMXApplication.Interfaces;
using DMXDomain.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace DMXApplication.Services
{
    public class AssignmentService(UserManager<AppUser> userManager,
                                   IAssignmentRepository assignmentRepository): IAssignmentService
    {
        private readonly UserManager<AppUser> usm = userManager;

        private readonly IAssignmentRepository assRep = assignmentRepository;

        public async Task<int> Assign(AssignDTO assignDTO)
        {
            var assign = new Assignment
            {
                AssignedDate = DateTime.UtcNow,
                PublicId = assignDTO.PublicId,


            };
            await assRep.Assign(assign);
           return assignDTO.Id;
        }

        Task<int> IAssignmentService.Update(AssignDTO assignment)
        {
            throw new NotImplementedException();
        }

        Task<int> IAssignmentService.Delete(int id)
        {
            throw new NotImplementedException();
        }

        Task<int> IAssignmentService.Get(int id)
        {
            throw new NotImplementedException();
        }

      

    }
    } 