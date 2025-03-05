using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using BusinessLayer.Interface;
using ModelLayer.Model.Entities;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IGreetingBL _greetingBL;

        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]string? firstName = null, [FromQuery]string? lastName = null)
        {
            string message = _greetingBL.GetGreetingMessage(firstName, lastName);
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting fetched Successfully",
                Data = message
            };
            Logger.Info("GET request received");
            return Ok(responseModel);
        }

        /// <summary>
        /// Post method to send the greeting message
        /// </summary>
        [HttpPost]
        public IActionResult Post([FromBody] RequestModel requestModel)
        {
            string responseMessage = _greetingBL.SaveGreetingMessage(requestModel.Value);
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Post Request Processed",
                Data = responseMessage
            };
            Logger.Info("POST request received and processed.");
            return Ok(responseModel);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Greeting greeting = _greetingBL.GetGreetingById(id);
            if(greeting == null)
            {
                return NotFound(new { Success = false, Message = "Greeting not found" });
            }
            return Ok(new { Success = true, Data = greeting.Message });
        }
    }
}