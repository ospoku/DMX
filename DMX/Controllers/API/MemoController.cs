using DMX.Services;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers.API
{
   

[ApiController]
    [Route("api/memo")]
    public class MemoController : ControllerBase
    {
        [HttpPost("autocomplete")]
        public IActionResult AutoComplete([FromBody] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return BadRequest("Input cannot be empty");

            var prediction = MemoTextModel.PredictText(text);  // Calls ML Model
            return Ok(prediction);
        }
    }

}

