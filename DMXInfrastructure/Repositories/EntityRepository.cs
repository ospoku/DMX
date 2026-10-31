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

        public async Task Add<T>(T entity) where T : class
        {
             await dxc.AddAsync(entity);
        }

        public async Task  Delete<T>(T model) where T : class
        {
            await Task.Run(() => dxc.Remove(model));
       
        }

        public async Task Update<T>(T model) where T : class
        {
            dxc.Update(model);
            await dxc.SaveChangesAsync();   
        }
        Task<int> IEntityRepository.SaveChangesAsync(string userId)
        {
            throw new NotImplementedException();
        }


       
    }

   
}

