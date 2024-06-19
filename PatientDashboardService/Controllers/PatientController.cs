using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using PatientDashboardService.Services.IService;
using PatientDashboardService.Models.DTO;

namespace PatientDashboardService.Controllers
{
  
        [Authorize(Roles = "Patient")]
        [Route("api/Patient")]
        [ApiController]
        public class PatientController : ControllerBase
        {
            private readonly IPatientService _patientService;
            private readonly ILogger<PatientController> _logger;

            public PatientController(IPatientService patientService, ILogger<PatientController> logger)
            {
                _patientService = patientService;
                _logger = logger;
            }

            [HttpPost("create")]
            public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDTO createPatientDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    var result = await _patientService.CreatePatientAsync(createPatientDto);
                    if (result > 0)
                    {
                        return Ok(new { success = true, message = "Patient created successfully" });
                    }
                    return BadRequest(new { success = false, message = "Error creating patient" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating patient");
                    return StatusCode(500, new { success = false, message = "Internal server error" });
                }
            }

            [HttpPut("update")]
            public async Task<IActionResult> UpdatePatient([FromBody] UpdatePatientDTO updatePatientDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    var result = await _patientService.UpdatePatientAsync(updatePatientDto);
                    if (result > 0)
                    {
                        return Ok(new { success = true, message = "Patient updated successfully" });
                    }
                    return BadRequest(new { success = false, message = "Error updating patient" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating patient");
                    return StatusCode(500, new { success = false, message = "Internal server error" });
                }
            }

            [HttpGet("get")]
            public async Task<IActionResult> GetPatient()
            {
                try
                {
                    var patient = await _patientService.GetPatientByUserIdAsync();
                    if (patient != null)
                    {
                        return Ok(patient);
                    }
                    return NotFound(new { success = false, message = "Patient not found" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving patient");
                    return StatusCode(500, new { success = false, message = "Internal server error" });
                }
            }
        }
}
