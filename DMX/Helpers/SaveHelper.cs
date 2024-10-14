using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static DMX.Constants.Permissions;

namespace DMX.Helpers
{
    

    public static class SaveHelper
    {
        private static readonly XContext? dcx;
        private static readonly INotyfService? notyf;
       private static readonly UserManager<AppUser>? usm;
       
        
        private static async Task<bool> SaveEntity<T>(T entity,string userId ) where T : class
        {
            try
            {
                dcx.Set<T>().Add(entity);
                if (await dcx.SaveChangesAsync(userId) > 0)
                {
                    notyf.Success("Record successfully saved!!!", 5);
                    return true;
                }
                else
                {
                    notyf.Error("Error, Record could not be saved!!!", 5);
                    return false;
                }
            }
            catch (Exception ex)
            {
                notyf.Error("An error occurred: " + ex.Message, 5);
                return false;
            }
        }
    }
}
