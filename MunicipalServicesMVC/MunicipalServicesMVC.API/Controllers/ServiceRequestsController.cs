using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVC.Application.Dtos;
using MunicipalServicesMVC.Application.Services;

namespace MunicipalServicesMVC.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestService _requestService;

        public ServiceRequestsController(
            IServiceRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] CreateServiceRequestDto dto)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(new
                {
                    message = "سجّلي الدخول من جديد"
                });
            }

            try
            {
                var request = _requestService.Create(userId, dto);

                return StatusCode(
                    StatusCodes.Status201Created,
                    request);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new
                {
                    message = exception.Message
                });
            }
        }

        [HttpGet("my")]
        public IActionResult MyRequests()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(new
                {
                    message = "سجّلي الدخول من جديد"
                });
            }

            return Ok(_requestService.GetMyRequests(userId));
        }

        private bool TryGetUserId(out int userId)
        {
            return int.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out userId);
        }
    }
}