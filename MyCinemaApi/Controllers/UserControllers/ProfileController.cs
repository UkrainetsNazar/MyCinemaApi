using Cinema.Application.UseCases;
using Cinema.Infrastructure.ExternalServices;
using Cinema.Presentation.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Cinema.Domain.Entities;
using Cinema.Application.UseCases.AuthServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Cinema.Presentation.Controllers.UserControllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly int DurationTime = 2;
        private readonly UseCaseManager _useCaseManager;
        private readonly ICacheService _cache;

        public ProfileController(
            UserManager<User> userManager,
            UseCaseManager useCaseManager,
            ICacheService cache,
            ITokenService authService)
        {
            _userManager = userManager;
            _useCaseManager = useCaseManager;
            _cache = cache;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var userCheck = User;
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var cacheKey = $"user_profile_{userId}";
                var cachedProfile = _cache.Data<object>(cacheKey);

                if (cachedProfile != null)
                {
                    stopwatch.Stop();
                    return Ok(ResponseCreator.Success(cachedProfile, 200, stopwatch.Elapsed.TotalMilliseconds));
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound("User not found");

                var tickets = await _useCaseManager.GetUserTicketsHandler.HandleAsync(userId);

                var userProfile = new
                {
                    user.UserName,
                    user.Email,
                    Tickets = tickets
                };

                _cache.SetData(cacheKey, userProfile, DurationTime);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success(userProfile, 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }

        [HttpPut("update-username")]
        public async Task<IActionResult> UpdateUserName([FromBody] string userName)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound("User not found");

                user.UserName = userName;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    return BadRequest(ResponseCreator.Error<object>("Failed to update username", 400, stopwatch.Elapsed.TotalMilliseconds));

                var cacheKey = $"user_profile_{userId}";
                _cache.ClearDataByPattern(cacheKey);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success("Username updated successfully", 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound("User not found");

                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword!, request.NewPassword!);
                if (!result.Succeeded)
                    return BadRequest(ResponseCreator.Error<object>("Failed to update password", 400, stopwatch.Elapsed.TotalMilliseconds));

                var cacheKey = $"user_profile_{userId}";
                _cache.ClearDataByPattern(cacheKey);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success("Password updated successfully", 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }
    }
}
