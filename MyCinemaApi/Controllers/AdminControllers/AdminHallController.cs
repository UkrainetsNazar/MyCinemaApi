using Cinema.Application.DTO.HallDTOs;
using Cinema.Application.UseCases;
using Cinema.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Presentation.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/hall")]
    public class AdminHallController : ControllerBase
    {
        private readonly UseCaseManager _useCaseManager;
        private readonly ICacheService _cache;

        public AdminHallController(UseCaseManager useCaseManager, ICacheService redisCacheService)
        {
            _useCaseManager = useCaseManager;
            _cache = redisCacheService;
        }   

        [HttpPost]
        public async Task<IActionResult> CreateHall([FromBody] CreateHallDTO request)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var hall = await _useCaseManager.CreateHallHandler.HandleAsync(request.HallNumber, request.RowCount, request.SeatsPerRow);
                stopwatch.Stop();
                return Ok(ResponseCreator.Success(hall, 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }
    }
}
