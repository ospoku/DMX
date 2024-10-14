using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace DMX.Helpers
{
    public  class SaveHelper
    {
        private readonly  XContext dcx;
        private  readonly  INotyfService notyf;
    

        public SaveHelper(XContext context, INotyfService notyfService)
        {
            XContext dcx = context?? throw new ArgumentNullException(nameof(dcx));
            notyf = notyfService?? throw new ArgumentNullException(nameof(notyf));


        }
       
        
        public async Task<bool> SaveEntity<T>(T entity,string userId ) where T : class
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
