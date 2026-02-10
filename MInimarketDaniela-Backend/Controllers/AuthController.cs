using Microsoft.AspNetCore.Mvc;
using MInimarketDaniela_Backend.DTOs;
using MInimarketDaniela_Backend.Services;

namespace MInimarketDaniela_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            try
            {
                var user = await _userService.RegisterAsync(userDto);
                return Ok(new
                {
                    message = "User registered successfully",
                    user
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Error registering user",
                    error = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _userService.LoginAsync(loginDto);
                return Ok(new
                {
                    message = "Login successful",
                    token
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    error = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = "Invalid credentials",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Error during login",
                    error = ex.Message
                });
            }
        }
    }
}
