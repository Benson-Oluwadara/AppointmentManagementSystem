//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Slot_Service.Models;
//using Slot_Service.Models.DTO;
//using Slot_Service.Service.IService;
//using System.Net;
//using System.Threading.Tasks;

//namespace Slot_Service.Controllers
//{

//    //[ApiExplorerSettings(IgnoreApi = true)] // Add this attribute to exclude the controller from Swagger

//    [Route("api/SlotService")]
//    [ApiController]
//    [Authorize(Roles = "Doctor")]
//    public class SlotServiceController : ControllerBase
//    {
//        private readonly ILogger<SlotServiceController> _logger;
//        private readonly ISlotService _slotService;

//        public SlotServiceController(ILogger<SlotServiceController> logger, ISlotService slotService)
//        {
//            _logger = logger;
//            _slotService = slotService;
//        }

//        [HttpPost("CreateSlot")]
//        public async Task<IActionResult> CreateSlot([FromBody] CreateSlotDTO createSlotDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    _logger.LogError("CreateSlotDTO object sent from client is null.");
//                    return BadRequest(new APIResponse(HttpStatusCode.BadRequest, false, null, GetModelStateErrorMessages()));
//                }
//                var result = await _slotService.CreateSlotAsync(createSlotDto);
//                return CreatedAtAction(nameof(CreateSlot), new { id = result }, result);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while creating the Slot");
//                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the Slot");
//            }
//        }
//        private List<string> GetModelStateErrorMessages()
//        {
//            _logger.LogWarning("Model state is invalid");
//            return ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
//        }
//        //[HttpGet]
//        //public async Task<IActionResult> GetAllSlots()
//        //{
//        //    try
//        //    {
//        //        var slots = await _slotService.GetAllSlotsAsync();
//        //        return Ok(slots);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _logger.LogError(ex, "An error occurred while retrieving the slots.");
//        //        return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the slots.");
//        //    }
//        //}

//        //[HttpGet("{id:int}")]
//        //public async Task<IActionResult> GetSlotById(int id)
//        //{
//        //    try
//        //    {
//        //        var slot = await _slotService.GetSlotByIdAsync(id);
//        //        if (slot == null)
//        //        {
//        //            return NotFound();
//        //        }
//        //        return Ok(slot);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _logger.LogError(ex, "An error occurred while retrieving the slot by ID.");
//        //        return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the slot.");
//        //    }
//        //}
//        [HttpGet("{code}")]
//        public async Task<IActionResult> GetSlotByCode()
//        {
//            try
//            {
//                var slot = await _slotService.GetSlotByCodeAsync();
//                if (slot == null)
//                {
//                    return NotFound();
//                }
//                return Ok(slot);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while retrieving the slot by Code.");
//                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the slot.");
//            }
//        }


//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateSlot(String SlotCodeName,int id, [FromBody] CreateSlotDTO updateSlotDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    _logger.LogError("UpdateSlotDTO object sent from client is invalid.");
//                    return BadRequest(new APIResponse(HttpStatusCode.BadRequest, false, null, GetModelStateErrorMessages()));
//                }

//                var success = await _slotService.UpdateSlotAsync(SlotCodeName, id, updateSlotDto);
//                if (!success)
//                {
//                    return NotFound();
//                }

//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while updating the slot.");
//                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the slot.");
//            }
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteSlot(string SlotCodeName, int id)
//        {
//            try
//            {
//                var success = await _slotService.DeleteSlotAsync(SlotCodeName,id);
//                if (!success)
//                {
//                    return NotFound();
//                }

//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while deleting the slot.");
//                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the slot.");
//            }
//        }



//    }

//}



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Slot_Service.Models;
using Slot_Service.Models.DTO;
using Slot_Service.Service.IService;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Slot_Service.Service.Service;

namespace Slot_Service.Controllers
{
    [Route("api/SlotService")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class SlotServiceController : ControllerBase
    {
        private readonly ILogger<SlotServiceController> _logger;
        private readonly ISlotService _slotService;
        private readonly AuthService _authService;

        public SlotServiceController(ILogger<SlotServiceController> logger, ISlotService slotService, AuthService authService)
        {
            _logger = logger;
            _slotService = slotService;
            _authService = authService;
        }

        [HttpPost("CreateSlot")]
        public async Task<IActionResult> CreateSlot([FromBody] CreateSlotDTO createSlotDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid CreateSlotDTO object sent from client.");
                    return BadRequest(new APIResponse(HttpStatusCode.BadRequest, false, null, GetModelStateErrorMessages()));
                }

                var userSlotCodeName = _authService.GetCurrentUserId();
                var result = await _slotService.CreateSlotAsync(createSlotDto, userSlotCodeName);
                return CreatedAtAction(nameof(CreateSlot), new { id = result.SlotCodeName }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the Slot.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the Slot.");
            }
        }

        [HttpGet("GetAllSlots")]
        public async Task<IActionResult> GetAllSlots()
        {
            try
            {
                var userSlotCodeName = _authService.GetCurrentUserId();
                var slots = await _slotService.GetSlotByCodeAsync(userSlotCodeName);
               
                foreach (var slot in slots)
                {
                    Console.WriteLine($"{slot.SlotCodeName}{slot.DateTime}{slot.availability}{slot.SlotId}");
                }
                return Ok(slots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the slots.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the slots.");
            }
        }

        [HttpGet("GetSlotById/{id:int}")]
        public async Task<IActionResult> GetSlotById(int id)
        {
            try
            {
                var userSlotCodeName = _authService.GetCurrentUserId();
                var slot = await _slotService.GetSlotByIdAndCodeAsync(id, userSlotCodeName);
                if (slot == null)
                {
                    return NotFound();
                }
                return Ok(slot);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the slot by ID.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the slot.");
            }
        }

        [HttpPut("UpdateSlot/{id:int}")]
        public async Task<IActionResult> UpdateSlot(int id, [FromBody] CreateSlotDTO updateSlotDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid UpdateSlotDTO object sent from client.");
                    return BadRequest(new APIResponse(HttpStatusCode.BadRequest, false, null, GetModelStateErrorMessages()));
                }

                var userSlotCodeName = _authService.GetCurrentUserId();
                var success = await _slotService.UpdateSlotAsync(userSlotCodeName, id, updateSlotDto);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the slot.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the slot.");
            }
        }

        [HttpDelete("DeleteSlot/{id:int}")]
        public async Task<IActionResult> DeleteSlot(int id)
        {
            try
            {
                var userSlotCodeName = _authService.GetCurrentUserId();
                var success = await _slotService.DeleteSlotAsync(userSlotCodeName, id);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the slot.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the slot.");
            }
        }

        private List<string> GetModelStateErrorMessages()
        {
            _logger.LogWarning("Model state is invalid.");
            return ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        }
    }
}
