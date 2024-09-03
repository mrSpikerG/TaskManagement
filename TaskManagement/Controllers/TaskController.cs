using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public Task<IActionResult> Get()
        //{
           
        //}
    }
}
