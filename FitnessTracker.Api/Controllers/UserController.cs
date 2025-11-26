using System.Security.Claims;
using FitnessTracker.Dtos;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
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
        // GET return all users
        //Requires Authorization (ex: Bearer jwt_token)
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GatherAllUser()
        {
            var users = await _userManager.Users.ToListAsync();
            if (User == null)
            {
                return BadRequest("No Users in Database");
            }

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GatherSpecificUserInfo(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest("No User in the Database");
            }

            var returnUserInfo = new ReturnUserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                email = user.Email,
                DailyCalorieGoal = user.DailyCalorieGoal
            };

            return Ok(returnUserInfo);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> EditUserInfo([FromBody] UpdateUserDto dto)
        {
            //Had to add User Id to claims in the AuthController
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Invalid token or user not logged in");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("No User in the Database");
            }

            if (!String.IsNullOrEmpty(dto.FirstName))
            {
                user.FirstName = dto.FirstName;
            }

            if (!String.IsNullOrEmpty(dto.LastName))
            {
                user.LastName = dto.LastName;
            }

            if (dto.DailyCalorieGoal.HasValue)
            {
                user.DailyCalorieGoal = dto.DailyCalorieGoal.Value;
            }

            if (!String.IsNullOrEmpty(dto.email) && dto.email != user.Email)
            {
                var result = await _userManager.SetEmailAsync(user, dto.email);
                if (!result.Succeeded)
                {
                    BadRequest(result.Errors);
                }
            }

            //We have to get a password reset token before we can update password due to Identity
            if (!String.IsNullOrEmpty(dto.password))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var newPassword = await _userManager.ChangePasswordAsync(user, resetToken, dto.password);

                if (!newPassword.Succeeded)
                {
                    BadRequest(newPassword.Errors);
                }
            }

            var updatedUserResult = await _userManager.UpdateAsync(user);
            if (!updatedUserResult.Succeeded)
            {
                BadRequest(updatedUserResult.Errors);
            }
            
            return Ok(new
            {
                message = "User updated successfully",
                user = new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.DailyCalorieGoal
                }
            });
        }
    }
}