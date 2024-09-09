using DataAccess.DTOs;
using LogicLayer.Interfaces;
using LogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("tasks/")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;

        // Constructor to inject necessary services
        public TaskController(ITaskService taskService, IUserService userService, ILogger<TaskController> logger)
        {
            _userService = userService;
            _taskService = taskService;
            _logger = logger;
        }

        // Endpoint for creating a task
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTask([FromBody] TaskDTO taskDto)
        {

            var userId = await _userService.GetUserIdByUsernameAsync(User.Identity.Name);
            if (userId == null)
            {
                _logger.LogWarning("CreateTask: Undefined user tried to create a task.");
                return BadRequest("Undefined user");
            }

            var task = await _taskService.CreateTaskAsync(taskDto, (Guid)userId);
            _logger.LogInformation($"CreateTask: Task '{taskDto.Title}' created for user with ID {userId}.");
            return Ok(task);
        }


        // Endpoint for getting tasks with filtering and pagination options
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTasks([FromQuery] TaskFilterDTO filterDto, [FromQuery] PaginationDTO paginationDto)
        {
            var userId = await _userService.GetUserIdByUsernameAsync(User.Identity.Name);
            if (userId == null)
            {
                _logger.LogWarning("GetTask: Undefined user tried to create a task.");
                return BadRequest("Undefined user");
            }
            var tasks = await _taskService.GetTasksAsync((Guid)userId, filterDto, paginationDto);
            _logger.LogInformation($"GetTasks: Retrieved tasks for user with ID {userId}.");
            return Ok(tasks);
        }

        // Endpoint for getting a specific task by ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTask(Guid id)
        {
            var userId = await _userService.GetUserIdByUsernameAsync(User.Identity.Name);
            if (userId == null)
            {
                _logger.LogWarning("GetTask: Undefined user tried to create a task.");
                return BadRequest("Undefined user");
            }
            var task = await _taskService.GetTaskByIdAsync(id, (Guid)userId);
            if (task == null)
            {
                _logger.LogWarning($"GetTask: Task with ID {id} not found for user with ID {userId}.");
                return NotFound();
            }
            _logger.LogInformation($"GetTask: Retrieved task '{task.Title}' for user with ID {userId}.");
            return Ok(task);
        }

        // Endpoint for updating an existing task
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskDTO taskDto)
        {
            var userId = await _userService.GetUserIdByUsernameAsync(User.Identity.Name);
            if (userId == null)
            {
                _logger.LogWarning("UpdateTask: Undefined user tried to create a task.");
                return BadRequest("Undefined user");
            }
            var updatedTask = await _taskService.UpdateTaskAsync(id, taskDto, (Guid)userId);
            if (updatedTask == null)
            {
                _logger.LogWarning($"UpdateTask: Task with ID {id} not found for user with ID {userId}.");
                return NotFound();
            }
            _logger.LogInformation($"UpdateTask: Task '{updatedTask.Title}' updated for user with ID {userId}.");
            return Ok(updatedTask);
        }

        // Endpoint for deleting a task by ID
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = await _userService.GetUserIdByUsernameAsync(User.Identity.Name);
            if (userId == null)
            {
                _logger.LogWarning("DeleteTask: Undefined user tried to create a task.");
                return BadRequest("Undefined user");
            }
            var result = await _taskService.DeleteTaskAsync(id, (Guid)userId);
            if (!result)
            {
                _logger.LogWarning($"DeleteTask: Task with ID {id} not found for user with ID {userId}.");
                return NotFound();
            }
            _logger.LogInformation($"DeleteTask: Task with ID {id} deleted for user with ID {userId}.");
            return Ok();
        }
    }
}
