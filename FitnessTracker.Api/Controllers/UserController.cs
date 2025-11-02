using FitnessTracker.Dtos;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto dto)
        {
            //check if email is already in db
            var isUserInputted = await _userManager.FindByEmailAsync(dto.email);
            if (isUserInputted != null)
            {
                return BadRequest("User is already created");
            }
            //if no then post

            var user = new User
            {
                UserName = dto.email,
                Email = dto.email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DailyCalorieGoal = dto.DailyCalorieGoal
            };

            var result = await _userManager.CreateAsync(user, dto.password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Return a simple response
            return Ok(new
            {
                Message = "User created successfully",
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.DailyCalorieGoal
            });
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return BadRequest("User does not exist!");
            }

            var userDeleted = await _userManager.DeleteAsync(user);

            if (!userDeleted.Succeeded)
            {
                return BadRequest(userDeleted.Errors);
            }

            return Ok(new
            {
                Message = "User Deleted",
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
            });
        }
    }
}