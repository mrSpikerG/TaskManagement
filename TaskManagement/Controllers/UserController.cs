using DataAccess.DTOs;
using LogicLayer.Interfaces;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("users/")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        // Constructor to inject the user service and logger
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // Endpoint for user registration
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            try
            {
                var result = await _userService.RegisterUserAsync(registerDto);
                _logger.LogInformation($"User '{registerDto.Username}' registered successfully.");
                return Ok(result);
            } catch (Exception ex)
            {
                _logger.LogError($"Error occurred during registration for user '{registerDto.Username}': {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        // Endpoint for user login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            try
            {
                var token = await _userService.AuthenticateUserAsync(loginDto);
                _logger.LogInformation($"User '{loginDto.Username}' logged in successfully.");
                return Ok(new { Token = token });
            } catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning($"Unauthorized access attempt for user '{loginDto.Username}': {ex.Message}");
                return Unauthorized(ex.Message);
            } catch (Exception ex)
            {
                _logger.LogError($"Error occurred during login for user '{loginDto.Username}': {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
