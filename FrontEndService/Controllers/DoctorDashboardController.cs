using Dashboard_Service.Models.DTO;
using Dashboard_Service.Services.IService;
using FrontEndService.Models;
using FrontEndService.Models.WebDTO.BioDTO;
using FrontEndService.Models.WebDTO.SlotDTO;
using FrontEndService.Services.IServices;
using FrontEndService.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace FrontEndService.Controllers
{

    public class DoctorDashboardController : Controller
    {
        private readonly IDoctorDashboardService _DocotorDashboardService;
        private readonly IDoctorSlotService _DocotorSlotService;
        public DoctorDashboardController(IDoctorDashboardService DocotorDashboardService, IDoctorSlotService docotorSlotService)
        {
            _DocotorDashboardService = DocotorDashboardService;
            _DocotorSlotService = docotorSlotService;
        }
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LoadBio()
        {
            var bio = await _DocotorDashboardService.GetBioByUserIdAsync<BioDTO>();
            return PartialView("_Bio", bio);
        }
        [HttpGet]
        public async Task<IActionResult> LoadEditBio()
        {
            var bio = await _DocotorDashboardService.GetBioByUserIdAsync<BioDTO>();
            return PartialView("_EditBio", bio);
        }
        [HttpGet]
        public IActionResult LoadAppointments()
        {
            // Retrieve appointments from your service if needed
            return PartialView("_Appointments");
        }
        [HttpGet]
        public async Task<IActionResult> LoadAvailability(SlotDTO slotdto)
        {

            var slot = await _DocotorSlotService.CreateSlotAsync<SlotDTO>(slotdto);
            return PartialView("_Availability", slot);
        }


        
        [HttpPost]
        public async Task<IActionResult> EditBio(BioDTO bioDTO)
        {
            if (ModelState.IsValid)
            {
                await _DocotorDashboardService.UpdateBioAsync<BioDTO>(bioDTO);
                return RedirectToAction("Dashboard");
            }
            return PartialView("_EditBio", bioDTO);
        }

        [HttpGet]
        public async Task<IActionResult>  SetAvailability()
        {
            var slots = await _DocotorSlotService.GetAllSlot<List<SlotDTO>>();
            // Print out the slots to the console
            Console.WriteLine("Available Slots:");
            foreach (var slot in slots)
            {
                Console.WriteLine($"Slot id:{slot.SlotId} Slot DateTime: {slot.DateTime}");
            }
            return View(slots);
        }
        
        

        [HttpPost]
        public async Task<IActionResult> CreateSlot(SlotDTO slotDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Parse the DateTime string from the form into a DateTime object
                    if (DateTime.TryParseExact(slotDTO.DateTime, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
                    {
                        slotDTO.DateTime = parsedDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); // Convert to desired format
                        Console.WriteLine("Slot date time:"+slotDTO.DateTime.ToString());
                    }
                    else
                    {
                        throw new Exception("Invalid DateTime format from the form.");
                    }

                    var result = await _DocotorSlotService.CreateSlotAsync<SlotDTO>(slotDTO);

                    // Assuming CreateSlotAsync returns the created slot data or success status
                    if (result != null)  // Modify this condition based on your service response
                    {
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to create slot. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error creating slot: " + ex.Message);
                }
            }

            // If ModelState is not valid or creation fails, return to the SetAvailability view
            return View("SetAvailability", slotDTO);
        }
       
        // POST: /DoctorDashboard/UpdateSlot/{id}
        [HttpPost]
        public async Task<IActionResult> UpdateSlot(int id, SlotDTO updateSlotDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                    //_logger.LogError("Invalid UpdateSlotDTO object sent from client.");
                    //return BadRequest(new WebAPIResponse(HttpStatusCode.BadRequest, false, null, "Validation error message", "Error"));

                }

                var success = await _DocotorSlotService.UpdateSlotAsync<SlotDTO>(id, updateSlotDto);
                //if (!success)
                //{
                //    return NotFound();
                //}

                return RedirectToAction("SetAvailability");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while updating the slot.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the slot.");
            }
        }

        // POST: /DoctorDashboard/DeleteSlot/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteSlot(int id)
        {
            try
            {
                var success = await _DocotorSlotService.DeleteSlotAsync<SlotDTO>(id);
                //if (!success)
                //{
                //    return NotFound();
                //}

                return RedirectToAction(nameof(SetAvailability));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while deleting the slot.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the slot.");
            }
        }

        // Simplified EditSlot method
        [HttpPost]
        public async Task<IActionResult> EditSlot(int id, SlotDTO updateSlotDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var success = await _DocotorSlotService.UpdateSlotAsync<SlotDTO>(id, updateSlotDto);
                //if (!success)
                //{
                //    return NotFound();
                //}

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the slot.");
            }
        }

        // Simplified EditSlotModal method
        [HttpGet]
        public async Task<IActionResult> EditSlotModal(int id)
        {
            var slot = await _DocotorSlotService.GetSlotAsync<SlotDTO>(id);
            if (slot == null)
            {
                return NotFound();
            }

            return PartialView("_EditSlotModal", slot);
        }




    }
}
