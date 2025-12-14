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
    [Route("api/app/cv")]
    [Authorize]
    public class CVController : ControllerBase
    {
        private readonly ICVService _cvService;

        public CVController(ICVService cvService)
        {
            _cvService = cvService;
        }

        // POST: api/app/cv
        [HttpPost]
        public async Task<ActionResult<CVDto>> CreateAsync([FromBody] CreateCVDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _cvService.CreateCVAsync(input);
            return Ok(result);
        }

        // PUT: api/app/cv/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<CVDto>> UpdateAsync(Guid id, [FromBody] UpdateCVDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _cvService.UpdateCVAsync(id, input);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/app/cv/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CVDto>> GetAsync(Guid id)
        {
            var result = await _cvService.GetCVAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/app/cv/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<CVDto>>> GetUserCVsAsync(Guid userId)
        {
            var result = await _cvService.GetUserCVsAsync(userId);
            return Ok(result);
        }

        // DELETE: api/app/cv/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _cvService.DeleteCVAsync(id);
            return NoContent();
        }

        // GET: api/app/cv/public
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PublicCVDto>>> GetPublicCVsAsync(int skip = 0, int take = 10)
        {
            var result = await _cvService.GetPublicCVsAsync(skip, take);
            return Ok(result);
        }

        // GET: api/app/cv/public/{userId}/count
        [HttpGet("public/{userId}/count")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> GetUserCVCountAsync(Guid userId)
        {
            var result = await _cvService.GetUserCVCountAsync(userId);
            return Ok(result);
        }
    }
}
