using DMXApplication.Interfaces;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DMXInfrastructure.Repositories
{ 
    public class EntityRepository(XContext dxc) : IEntityRepository
    {

        public void Add<T>(T entity) where T : class
        {
            dxc.Add(entity);
        }

        public void Delete<T>(T model) where T : class
        {
            dxc.Remove(model);
       
        }

        public void Update<T>(T model) where T : class
        {
            dxc.Update(model);
        
        }
     public   Task<int> SaveChangesAsync(string userId)
        {
            return dxc.SaveChangesAsync(userId);
        }


       
    }

   
}

