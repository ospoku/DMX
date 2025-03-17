using DMX.Data;
using DMX.Models;
using DMX.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.Controllers
{
    public class MessageController(IMessageService messageService, UserManager<AppUser> userManager, XContext context) : Controller
    {
        readonly IMessageService ms = messageService;
        List<Message> oMessages = [];
        readonly UserManager<AppUser> usm = userManager;
        readonly XContext ctx = context;

        public IActionResult AllMessages()
        {
            return View();
        }
        public JsonResult GetMessages(bool bIsGetOnlyRead = false)
        {

            Task<AppUser> Receiver = usm.FindByNameAsync(HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("Name")).Value);
            string ReceiverId = Receiver.Result.UserName;
            oMessages = new List<Message>();
            oMessages = ms.GetMessages(ReceiverId, bIsGetOnlyRead);


            return Json(oMessages);
        }
        [HttpGet]
        public IActionResult UserMessages()
        {
            return ViewComponent("UserMessages");
        }

        //[HttpPost]
        //public JsonResult UpdateMessage(string Id)
        //{

        //    var msg = ctx.Messages.Where(m => m.MessageId == Id).FirstOrDefault();
        //    if (msg != null)
        //    {
        //        msg.MessageId = Id;
        //        msg.IsRead = 1;
        //        ctx.SaveChanges();
        //    }
        //    return Json(msg);
        //}
        [HttpPost]
        public async Task<IActionResult> UpdateMessage(string Id)
        {

            var msg = ctx.Messages.Where(m => m.MessageId == Id).FirstOrDefault();
            if (msg != null)
            {
                msg.MessageId = Id;
                msg.IsRead = 1;
                 ctx.Messages.Attach(msg);
               ctx.Entry(msg).State = EntityState.Modified;
                ctx.SaveChanges();
            }
            return RedirectToAction("UserMessages");
        }
    }
}
