using DMXApplication.DTO;
using DMXApplication.Interfaces;
using DMXDomain.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace DMXApplication.Services
{
    public class AssignmentService(UserManager<AppUser> userManager,
                                   IAssignmentRepository assignmentRepository)
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


    }
    } 