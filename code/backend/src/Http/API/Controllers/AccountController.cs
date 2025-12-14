using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CVSystem.Application.Contracts.Services;
using CVSystem.Application.DTOs;

namespace CVSystem.Http.API.Controllers
{
    [ApiController]
    [Route("api/account")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/account/profile
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfileAsync()
        {
            // TODO: Replace with actual authenticated user id from claims
            var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var result = await _userService.GetUserByIdAsync(userId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/account/profile
        [HttpPut("profile")]
        public async Task<ActionResult<UserDto>> UpdateProfileAsync([FromBody] UpdateUserDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Replace with actual authenticated user id from claims
            var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var result = await _userService.UpdateUserAsync(userId, input);
            return Ok(result);
        }

        // GET: api/account/user-profile
        [HttpGet("user-profile")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfileAsync()
        {
            // TODO: Replace with actual authenticated user id from claims
            var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var result = await _userService.GetUserProfileAsync(userId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/account/user-profile
        [HttpPut("user-profile")]
        public async Task<ActionResult<UserProfileDto>> UpdateUserProfileAsync([FromBody] UpdateUserProfileDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Replace with actual authenticated user id from claims
            var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var result = await _userService.UpdateUserProfileAsync(userId, input);
            return Ok(result);
        }

        // ADMIN: GET: api/account/users
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsersAsync()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }

        // ADMIN: GET: api/account/users/{userId}
        [HttpGet("users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(Guid userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // ADMIN: DELETE: api/account/users/{userId}
        [HttpDelete("users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteUserAsync(Guid userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return Ok(result);
        }
    }
}

public class UpdateUserDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Nationality { get; set; }
}

public class UpdateUserProfileDto
{
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string ProfileImageUrl { get; set; }
    public string Bio { get; set; }
}
