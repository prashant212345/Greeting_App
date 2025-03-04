using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using BusinessLayer.Interface;

namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// Class providing API for Hellogreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        //private readonly IGreetingBL _greetingBL;

        //public HelloGreetingController(IGreetingBL greetingBL)
        //{
        //    _greetingBL = greetingBL;
        //}

        /// <summary>
        /// Get Method to get the greeting message
        /// </summary>
        /// <returns>Hello World!</returns>
        [HttpGet]
        public IActionResult Get()
        {
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Hello to Greeting App API Endpoint",
                Data = "Get Request Received!"
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
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Post Request Received successfully",
                Data = "Key:" + requestModel.Key + " Value:" + requestModel.Value
            };
            Logger.Info("POST request received and processed.");
            return Ok(responseModel);
        }

        /// <summary>
        /// Put method to update the greeting message
        /// </summary>
        [HttpPut]
        public IActionResult Put([FromBody] RequestModel requestModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "PUT request processed successfully. Greeting message updated.",
                Data = "Updated Key: " + requestModel.Key + ", Updated Value: " + requestModel.Value
            };
            Logger.Info("PUT request received and processed.");
            return Ok(responseModel);
        }

        /// <summary>
        /// Patch method to partially update the greeting message
        /// </summary>
        [HttpPatch]
        public IActionResult Patch([FromBody] RequestModel requestModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "PATCH request processed successfully. Greeting message partially updated.",
                Data = "Updated Key: " + requestModel.Key + ", Updated Value: " + requestModel.Value
            };
            Logger.Info("PATCH request received and processed.");
            return Ok(responseModel);
        }

        /// <summary>
        /// Delete method to remove a greeting message
        /// </summary>
        [HttpDelete]
        public IActionResult Delete([FromBody] RequestModel requestModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "DELETE request processed successfully. Greeting message deleted.",
                Data = "Deleted Key: " + requestModel.Key + ", Deleted Value: " + requestModel.Value
            };
            Logger.Info("DELETE request received and processed.");
            return Ok(responseModel);
        }
    }
}