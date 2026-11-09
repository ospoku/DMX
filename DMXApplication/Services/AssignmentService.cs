using DMXApplication.Interfaces;
using DMXDomain.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DMXApplication.Services
{
    public class AssignmentService(UserManager<AppUser> userManager,
                                   IAssignmentRepository assignmentRepository)
    {
        private readonly UserManager<AppUser> usm = userManager;

        private readonly IAssignmentRepository assRep = assignmentRepository;

        //public Task<bool> AddAssignee()
        //{
        //    assRep.AddAssignee



    }
    } 