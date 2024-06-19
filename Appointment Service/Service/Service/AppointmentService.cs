using Appointment_Service.Models;
using Appointment_Service.Models.DTO;
using Appointment_Service.Repository.IRepository;
using Appointment_Service.Service.IService;
using AutoMapper;
using Slot_Service.Models;
using Slot_Service.Service.IService;
using Slot_Service.Service.Service;
using static Dapper.SqlMapper;

namespace Appointment_Service.Service.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        private readonly ISlotServiceClient _slotserviceclient;

        //Constructor
        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper, ISlotServiceClient slotserviceclient)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _slotserviceclient = slotserviceclient;
        }

        public async Task<AppointmentDTO> CreateAppointmentAsync(CreateAppointmentDTO createAppointmentDto)
        {
            try
            {
                // Fetch available slots by SlotCode
                var availableSlots = await _slotserviceclient.GetSlotsByCodeAsync("string");

                // Check for a matching slot
                var requestedSlot = availableSlots.FirstOrDefault(slot => slot.DateTime == createAppointmentDto.DateTime && slot.Availability == "Available");

                if (requestedSlot != null)
                {
                    // Create a new appointment from the request
                    var appointment = new Appointment
                    {
                        DateTime = createAppointmentDto.DateTime,
                        CreatedDate = DateTime.UtcNow,
                        LastUpdatedDate = DateTime.UtcNow
                    };

                    // Save the appointment
                    var appointmentId = await _appointmentRepository.CreateAppointmentAsync(appointment);

                    // Retrieve the created appointment
                    var createdAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);

                    // Map the created appointment to DTO
                    var createdAppointmentDto = _mapper.Map<AppointmentDTO>(createdAppointment);
                    return createdAppointmentDto;
                }
                else
                {
                    throw new InvalidOperationException("The requested slot is not available.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error creating appointment: {ex.Message}");
                throw; // Re-throw the exception to propagate it further
            }
        }
        //public async Task<int> UpdateAppointmentToBookedAsync(string slotCodeName, DateTime appointmentDateTime)
        //{
        //    try
        //    {
        //        return await _appointmentRepository.UpdateAppointmentToBookedAsync(slotCodeName, appointmentDateTime);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        Console.WriteLine($"Error updating appointment to booked: {ex.Message}");
        //        throw; // Re-throw the exception to propagate it further
        //    }
        //}
        //public async Task<int> UpdateAppointmentToBookedAsync(UpdateAppointmentToBookedDTO updateAppointmentDto)
        //{
        //    return await _appointmentRepository.UpdateAppointmentToBookedAsync(updateAppointmentDto.SlotCodeName, updateAppointmentDto.DateTime);
        //}
        //public async Task<AppointmentDTO> CreateAppointmentAsync(CreateAppointmentDTO createAppointmentDto)
        //{
        //    try
        //    {
        //        // Fetch available slots by SlotCode
        //        var availableSlots = await _slotserviceclient.GetSlotsByCodeAsync("string");

        //        // Check for a matching slot
        //        var requestedSlot = availableSlots.FirstOrDefault(slot => slot.DateTime == createAppointmentDto.DateTime && slot.Availability == "Available");

        //        if (requestedSlot != null)
        //        {
        //            // Create a new appointment from the request
        //            var appointment = _mapper.Map<Appointment>(createAppointmentDto);
        //            appointment.DateTime = createAppointmentDto.DateTime;
        //            appointment.CreatedDate = DateTime.UtcNow;
        //            appointment.LastUpdatedDate = DateTime.UtcNow;

        //            // Save the appointment
        //            var appointmentId = await _appointmentRepository.CreateAppointmentAsync(appointment);

        //            // Retrieve the created appointment
        //            var createdAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);

        //            // Map the created appointment to DTO
        //            var createdAppointmentDto = _mapper.Map<AppointmentDTO>(createdAppointment);
        //            return createdAppointmentDto;
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("The requested slot is not available.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        Console.WriteLine($"Error creating appointment: {ex.Message}");
        //        throw; // Re-throw the exception to propagate it further
        //    }
        //}


        public async Task<IEnumerable<ReadAppointmentDTO>> GetAppointmentsBySlotCodeNameAsync(string slotCodeName)
        {
            return await _appointmentRepository.GetAppointmentsBySlotCodeNameAsync(slotCodeName);


        }
        public async Task<IEnumerable<Slot>> GetAvailableSlotsAsync()
        {
            var slots = await _appointmentRepository.GetAvailableSlotsAsync();
            Console.WriteLine("Available Slots:");
            foreach (var slot in slots)
            {
                Console.WriteLine($"{slot.SlotID} - {slot.DateTime}");
            }
            return slots;
        }
        public async Task<IEnumerable<Slot>> GetBookedSlotsByCodeNameAsync(string slotCodeName)
        {
            var slots = await _appointmentRepository.GetBookedSlotsByCodeNameAsync(slotCodeName);
            Console.WriteLine($"Booked Slots for {slotCodeName}:");
            foreach (var slot in slots)
            {
                Console.WriteLine($"{slot.SlotID} - {slot.DateTime}");
            }
            return slots;
        }
        public async Task UpdateAppointmentToBookedAsync(UpdateAppointmentToBookedDTO dto)
        {
            var availableSlots = await GetAvailableSlotsAsync();
            var bookedSlots = await GetBookedSlotsByCodeNameAsync(dto.SlotCodeName);

            var slotToUpdate = bookedSlots.FirstOrDefault(s => s.DateTime == dto.CurrentDateTime);
            if (slotToUpdate == null)
            {
                throw new Exception("No booked slot matches the provided date and time.");
            }

            var newSlot = availableSlots.FirstOrDefault(s => s.DateTime == dto.NewDateTime);
            if (newSlot == null)
            {
                throw new Exception("No available slot matches the new date and time.");
            }

            // Update the matched booked slot to available
            await _appointmentRepository.UpdateSlotToAvailableAsync(slotToUpdate.SlotID);

            // Update the new available slot to booked
            await _appointmentRepository.UpdateSlotToBookedAsync(newSlot.SlotID, dto.NewDateTime);
        }


        public async Task CancelAppointmentAsync(CancelAppointmentDTO dto)
        {
            await _appointmentRepository.CancelAppointmentAsync(dto);
        }
    }

}
