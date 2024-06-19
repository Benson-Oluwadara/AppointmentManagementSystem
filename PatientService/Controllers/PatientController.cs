using Microsoft.AspNetCore.Mvc;
using PatientService.Models.DTO;
using PatientService.Models;

using System.Collections.Generic;
using System.Threading.Tasks;
using PatientService.Services.IService;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Patient")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAllPatients()
    {
        var patients = await _patientService.GetAllPatientsAsync();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatientById(Guid id)
    {
        var patient = await _patientService.GetPatientByIdAsync(id);
        if (patient == null)
        {
            return NotFound();
        }
        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreatePatient([FromBody] CreatePatientDTO createPatientDto)
    {
        var patientId = await _patientService.CreatePatientAsync(createPatientDto);
        return Ok(patientId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePatient(Guid id, [FromBody] CreatePatientDTO updatePatientDto)
    {
        await _patientService.UpdatePatientAsync(id, updatePatientDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePatient(Guid id)
    {
        await _patientService.DeletePatientAsync(id);
        return NoContent();
    }
}
