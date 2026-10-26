
using DMXApplication.Interfaces;
using DMXDomain.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace DMXApplication.Services
{
    namespace DMXApplication.Services
    {
        public class EntityService(
            UserManager<AppUser> userManager,
            IEntityPersistence persistence)
        {
            private readonly UserManager<AppUser> usm = userManager;
            private readonly IEntityPersistence entPer = persistence;

            public async Task<bool> AddEntityAsync<T>(
                T entity,
                ClaimsPrincipal userClaim) where T : class
            {
                try
                {
                    var user = await usm.GetUserAsync(userClaim);

                    if (user == null)
                        return false;

                    var entityType = typeof(T);

                    entityType.GetProperty("CreatedBy")
                        ?.SetValue(entity, user.Id);

                    entityType.GetProperty("CreatedDate")
                        ?.SetValue(entity, DateTime.UtcNow);

                    entityType.GetProperty("IsDeleted")
                        ?.SetValue(entity, false);

                    entPer.Add(entity);

                    await entPer.SaveChangesAsync(user.Id);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public async Task<bool> EditEntityAsync<T>(
                T model,
                ClaimsPrincipal userClaim) where T : class
            {
                try
                {
                    var user = await usm.GetUserAsync(userClaim);

                    if (user == null)
                        return false;

                    var entityType = typeof(T);

                    entityType.GetProperty("ModifiedBy")
                        ?.SetValue(model, user.Id);

                    entityType.GetProperty("ModifiedDate")
                        ?.SetValue(model, DateTime.UtcNow);

                    entPer.Update(model);

                    await entPer.SaveChangesAsync(user.Id);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public async Task<bool> DeleteEntityAsync<T>(
                T model,
                ClaimsPrincipal userClaim) where T : class
            {
                try
                {
                    var user = await usm.GetUserAsync(userClaim);

                    if (user == null)
                        return false;

                    var entityType = typeof(T);

                    entityType.GetProperty("ModifiedBy")
                        ?.SetValue(model, user.Id);

                    entityType.GetProperty("ModifiedDate")
                        ?.SetValue(model, DateTime.UtcNow);

                    entityType.GetProperty("IsDeleted")
                        ?.SetValue(model, true);

                    entPer.Update(model);

                    await entPer.SaveChangesAsync(user.Id);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }

}
