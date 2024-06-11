using Microsoft.AspNetCore.Mvc;
using RateMyProject.Models;
using RateMyProject.Services;

namespace RateMyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userService.GetUserByCredentialsAsync(loginRequest.Username, loginRequest.Password);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            user.Password = null; // Remove the password before returning
            return Ok(user);
        }

        [HttpPut("changePassword/{id}")]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] ChangePasswordRequest request)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            if (user.Password != request.OldPassword)
            {
                return BadRequest(new { Message = "Old password is incorrect." });
            }

            await _userService.UpdatePasswordAsync(id, request.NewPassword);
            return Ok(new { Message = "Password changed successfully." });
        }

        [HttpPost("createUser")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            try
            {
                await _userService.CreateUserAsync(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("createMultipleUsers")]
        public async Task<ActionResult<List<User>>> CreateMultipleUsers(int startIndex, int count)
        {
            try
            {
                List<User> users = new List<User>();
                for (int i = startIndex; i < startIndex + count; i++)
                {
                    string username = "S" + i.ToString();
                    string password = username;

                    User user = new User
                    {
                        Username = username,
                        Password = password,
                        IsProfessor = false
                    };

                    users.Add(user);
                }

                await _userService.CreateMultipleUsersAsync(users);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("deleteUsersByIndexRange")]
        public async Task<IActionResult> DeleteUsersByIndexRange(int pocetniBrojIndeksa, int brojStudenata)
        {
            try
            {
                for (int i = 0; i < brojStudenata; i++)
                {
                    string username = "S" + (pocetniBrojIndeksa + i).ToString();
                    await _userService.DeleteUserByUsernameAsync(username);
                }
                return Ok("Korisnici su uspešno obrisani.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

    }
}
