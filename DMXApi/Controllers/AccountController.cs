
using Microsoft.AspNetCore.Mvc;

namespace DMXApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("AddUser")]
        public IActionResult AddUser()
        {
            return Ok("AddUser endpoint is working.");
        }

        [HttpPut("EditUser/{id}")]
        public IActionResult EditUser(string id)
        {
            return Ok(new
            {
                Message = "EditUser endpoint is working.",
                UserId = id
            });
        }

        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(string id)
        {
            return Ok(new
            {
                Message = "DeleteUser endpoint is working.",
                UserId = id
            });
        }

        [HttpGet("ManageUser/{id}")]
        public IActionResult ManageUser(string id)
        {
            return Ok(new
            {
                Message = "ManageUser endpoint is working.",
                UserId = id
            });
        }

        [HttpGet("ManageAll")]
        public IActionResult ManageAll()
        {
            return Ok("ManageAll endpoint is working.");
        }

        [HttpGet("Manage")]
        public IActionResult Manage()
        {
            return Ok("Manage endpoint is working.");
        }
    }
}
