using DMX.Data;
using DMX.DataProtection;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class EditPerDiem(XContext dContext):ViewComponent
    {
        public readonly XContext ctx=dContext;

       public  IViewComponentResult Invoke(string Id)
        {
            var perdiemToEdit = ctx.Users.Where(x => x.Id == @Encryption.Decrypt(Id)).Select(x => new EditPerdiemVM { 
                Id=Id,
                Amount= ctx.PerDiems.Where(a => a.UserId == x.Id).Select(a => a.Amount).SingleOrDefault(),
            Department=x.DepartmentId,
            Rank=x.RankId,
            Name=x.Fullname,
            }).FirstOrDefault();

            return View(perdiemToEdit);
        }


    }
}
