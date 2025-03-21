using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.DTO;

namespace Register_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegController : Controller
    {
        private readonly IGreetingBL _greetingBL;
        public RegController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var result = await _greetingBL.Register(model);
            if(result == "User already exists") 
                return BadRequest(new {message  = result});

            return Ok(new { message = result });
        }
    }
}
