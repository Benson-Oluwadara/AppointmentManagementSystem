using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Dashboard_Service.Models.DTO;
using Dashboard_Service.Services.IService;
using Microsoft.AspNetCore.Authorization;

namespace Dashboard_Service.Controllers
{
    [Authorize(Roles ="Doctor")]
    [Route("api/Bio")]
    [ApiController]
    public class BioController : ControllerBase
    {
        private readonly IBioService _bioService;

        public BioController(IBioService bioService)
        {
            _bioService = bioService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBio([FromBody] CreateBioDTO createBioDto)
        {
            var result = await _bioService.CreateBioAsync(createBioDto);
            if (result > 0)
            {
                return Ok(new { success = true, message = "Bio created successfully" });
            }
            return BadRequest(new { success = false, message = "Error creating bio" });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateBio([FromBody] CreateBioDTO updateBioDto)
        {
            var result = await _bioService.UpdateBioAsync(updateBioDto);
            if (result > 0)
            {
                return Ok(new { success = true, message = "Bio updated successfully" });
            }
            return BadRequest(new { success = false, message = "Error updating bio" });
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetBio()
        {
            var bio = await _bioService.GetBioByUserIdAsync();
            if (bio != null)
            {
                return Ok(bio);
            }
            return NotFound(new { success = false, message = "Bio not found" });
        }
    }
}
