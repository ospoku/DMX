using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Helpers;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DMX.Services
{
    public class EntityService
    {
        private readonly UserManager<AppUser> usm;
        private readonly INotyfService notyf;

        private readonly XContext dcx;

        public EntityService(UserManager<AppUser> userManager, XContext context, INotyfService notyfService)
        {
            usm = userManager;
            notyf = notyfService;
            dcx = context;
        }

        public async Task<bool> AddEntityAsync<T>(T entity, ClaimsPrincipal userClaim) where T : class
        {
            var user = await usm.GetUserAsync(userClaim);
            if (user == null)
            {
                notyf.Error("User is not authenticated.", 5);
                return false;
            }

            entity.GetType().GetProperty("CreatedBy")?.SetValue(entity, user.UserName);
            entity.GetType().GetProperty("CreatedDate")?.SetValue(entity, DateTime.UtcNow);

            try
            {
                dcx.Set<T>().Add(entity);
                if (await dcx.SaveChangesAsync(user.UserName) > 0)
                {
                   
                    return true;
                }
                else
                {
                   
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //public async Task<bool> EditEntityAsync<T>(T model, ClaimsPrincipal userClaim) where T : class
        //{
        //    var user = await usm.GetUserAsync(userClaim);
        //    if (user == null)
        //    {
        //        notyf.Error("User is not authenticated.", 5);
        //        return false;
        //    }

        //    var modelType = model.GetType();
        //    var modifiedByProp = modelType.GetProperty("ModifiedBy");
        //    var modifiedDateProp = modelType.GetProperty("ModifiedDate");

        //    if (modifiedByProp != null)
        //    {
        //        modifiedByProp.SetValue(model, user.UserName);
        //    }

        //    if (modifiedDateProp != null)
        //    {
        //        modifiedDateProp.SetValue(model, DateTime.UtcNow);
        //    }

        //    try
        //    {
        //        dcx.Set<T>().Update(model);

        //        if (await dcx.SaveChangesAsync(user.UserName) > 0)
        //        {
        //            notyf.Success("Record successfully updated!", 5);
        //            return true;
        //        }
        //        else
        //        {
        //            notyf.Error("Error, record could not be updated.", 5);
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        notyf.Error("An error occurred: " + ex.Message, 5);
        //        return false;
        //    }
        //}
       
        public async Task<bool> DeleteEntityAsync<T>(T model, ClaimsPrincipal userClaim) where T : class
        {
            var user = await usm.GetUserAsync(userClaim);
            if (user == null)
            {
                notyf.Error("User is not authenticated.", 5);
                return false;
            }
            model.GetType().GetProperty("ModifiedBy")?.SetValue(model, user.UserName);
            model.GetType().GetProperty("ModifiedDate")?.SetValue(model, DateTime.UtcNow);
            model.GetType().GetProperty("IsDeleted")?.SetValue(model,true);
            try
            {
                dcx.Set<T>().Update(model);

                if (await dcx.SaveChangesAsync(user.UserName) > 0)
                {
                    notyf.Success("Record successfully updated!", 5);
                    return true;
                }
                else
                {
                    notyf.Error("Error, record could not be updated.", 5);
                    return false;
                }
            }

            catch (Exception ex)
            {
                notyf.Error("An error occurred: " + ex.Message, 5);
                return false;
            }
        }

        public async Task<bool> EditEntityAsync<T>(T model, ClaimsPrincipal userClaim) where T : class
        {
            var user = await usm.GetUserAsync(userClaim);
            if (user == null)
            {
                notyf.Error("User is not authenticated.", 5);
                return false;
            }

            var modelType = model.GetType();
            var modifiedByProp = modelType.GetProperty("ModifiedBy");
            var modifiedDateProp = modelType.GetProperty("ModifiedDate");

            if (modifiedByProp != null)
            {
                modifiedByProp.SetValue(model, user.UserName);
            }

            if (modifiedDateProp != null)
            {
                modifiedDateProp.SetValue(model, DateTime.UtcNow);
            }

            try
            {
                dcx.Set<T>().Update(model);

                if (await dcx.SaveChangesAsync(user.UserName) > 0)
                {
                    notyf.Success("Record successfully updated!", 5);
                    return true;
                }
                else
                {
                    notyf.Error("Error, record could not be updated.", 5);
                    return false;
                }
            }
            catch (Exception ex)
            {
                notyf.Error("An error occurred: " + ex.Message, 5);
                return false;
            }
        }

        public async Task<bool> EditMemoAsync(string Id, EditMemoVM editMemoVM, ClaimsPrincipal userClaim)
        {
            var decryptedId = Encryption.Decrypt(Id);
            var updateThisMemo = await dcx.Memos.FirstOrDefaultAsync(a => a.MemoId == decryptedId);

            if (updateThisMemo == null)
            {
                notyf.Error("Memo not found", 5);
                return false;
            }

            updateThisMemo.Content = editMemoVM.Content;
            updateThisMemo.Title = editMemoVM.Title;

            if (!await EditEntityAsync(updateThisMemo, userClaim))
            {
                return false; // If entity update fails, stop here
            }

            // Remove existing memo assignments
            var existingAssignments = dcx.MemoAssignments.Where(x => x.MemoId == decryptedId);
            dcx.MemoAssignments.RemoveRange(existingAssignments);

            // Add new memo assignments
            var user = await usm.GetUserAsync(userClaim);
            foreach (var userId in editMemoVM.SelectedUsers)
            {
                dcx.MemoAssignments.Add(new MemoAssignment
                {
                    MemoId = updateThisMemo.MemoId,
                    AppUserId = userId,
                    CreatedBy = user?.UserName,
                    CreatedDate = DateTime.UtcNow,
                });
            }

            // Save changes after updating assignments
            return await dcx.SaveChangesAsync(user?.UserName) > 0;
        }
    }
}

