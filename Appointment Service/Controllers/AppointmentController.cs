using Appointment_Service.Models;
using Appointment_Service.Models.DTO;
using Appointment_Service.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace Appointment_Service.Controllers
{

    [Route("api/AppointmentService")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class AppointmentController : ControllerBase
    {
        

        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentService appointmentService)
        {
            _logger = logger;
            _appointmentService = appointmentService;
        }

        //Create Appoinments based on Available Slot 
        [HttpPost("CreateAppointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDTO createAppointmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for CreateAppointmentDTO.");
                    return BadRequest(new APIResponse(HttpStatusCode.BadRequest, false, null, GetModelStateErrorMessages()));
                }


                
                // Now you can use inputDateTime for further processing or validation

                // Call the service method to create the appointment
                var result = await _appointmentService.CreateAppointmentAsync(createAppointmentDto);

                //var result = await _appointmentService.CreateAppointmentAsync(createAppointmentDto);
                return CreatedAtAction(nameof(CreateAppointment), new { id = result }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the appointment.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the appointment.");
            }
        }

        ////Read All Appointmrnt Created by code
        //[HttpGet("GetBookedAppointmentsBySlotCodeName")]
        //public async Task<IActionResult> GetBookedAppointmentsBySlotCodeName(string slotCodeName)
        //{
        //    try
        //    {
        //        var result = await _appointmentService.GetAppointmentsBySlotCodeNameAsync(slotCodeName);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while fetching appointments by slot code name.");
        //        return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching appointments.");
        //    }
        //}
        //// POST api/AppointmentService/UpdateAppointmentToBooked
        
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots()
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync();
            return Ok(slots);
        }

        [HttpGet("booked-slots/{slotCodeName}")]
        public async Task<IActionResult> GetBookedSlotsByCodeName(string slotCodeName)
        {
            var slots = await _appointmentService.GetBookedSlotsByCodeNameAsync(slotCodeName);
            return Ok(slots);
        }

        [HttpPut("update-appointment")]
        public async Task<IActionResult> UpdateAppointmentToBooked([FromBody] UpdateAppointmentToBookedDTO updateAppointmentDto)
        {
            try
            {
                await _appointmentService.UpdateAppointmentToBookedAsync(updateAppointmentDto);
                return Ok(new { Message = "Appointment updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("cancel")]
        public async Task<ActionResult> CancelAppointment([FromBody] CancelAppointmentDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            try
            {
                await _appointmentService.CancelAppointmentAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        private List<string> GetModelStateErrorMessages()
        {
            _logger.LogWarning("Model state is invalid");
            return ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        }
    }
}