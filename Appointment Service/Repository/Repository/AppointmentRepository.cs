using Appointment_Service.Database.IDapperRepositorys;
using Appointment_Service.Models;
using Appointment_Service.Models.DTO;
using Appointment_Service.Repository.IRepository;
using AutoMapper;
using Slot_Service.Models.DTO;
using Slot_Service.Models;
using System.Data;
using System.Data.Common;

namespace Appointment_Service.Repository.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {

        private readonly IDapperRepository _dapperRepository;
        private readonly IMapper _mapper;



        public AppointmentRepository(IDapperRepository dapperRepository, IMapper mapper)
        {
            _dapperRepository = dapperRepository;
            _mapper = mapper;
        }
        public async Task<int> CreateAppointmentAsync(Appointment appointment)
        {
            // Step 1: Get the available slot
            var getSlotSql = @"
                SELECT TOP 1 [SlotID]
                FROM [dbo].[Slots]
                WHERE [DateTime] = @DateTime AND [Availability] = 'Available'
                ORDER BY [SlotID]";

            var slotParameters = new { DateTime = appointment.DateTime };
            var slotId = await _dapperRepository.QuerySingleOrDefaultAsync<int>(getSlotSql, slotParameters);

            if (slotId == 0)
            {
                throw new Exception("No available slot found for the specified date and time.");
            }

            // Step 2: Insert the appointment with the retrieved SlotID
            var insertAppointmentSql = @"
                INSERT INTO [dbo].[Appointments] (DateTime, CreatedDate, LastUpdatedDate, SlotID)
                VALUES (@DateTime, @CreatedDate, @LastUpdatedDate, @SlotID);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            var appointmentParameters = new
            {
                DateTime = appointment.DateTime,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow,
                SlotID = slotId
            };

            var appointmentId = await _dapperRepository.QuerySingleAsync<int>(insertAppointmentSql, appointmentParameters);

            // Optionally, update the slot's availability if required
            var updateSlotSql = @"
                UPDATE [dbo].[Slots]
                SET [Availability] = 'Booked'
                WHERE [SlotID] = @SlotID";

            await _dapperRepository.ExecuteAsync(updateSlotSql, new { SlotID = slotId });

            return appointmentId;
        }

        //    public async Task<int> CreateAppointmentAsync(CreateAppointmentDTO createAppointmentDto)
        //    {
        //        // Step 1: Get the available slot
        //        var getSlotSql = @"
        //SELECT TOP 1 [SlotID]
        //FROM [dbo].[Slots]
        //WHERE [DateTime] = @DateTime AND [Availability] = 'Available'
        //ORDER BY [SlotID]";

        //        var slotParameters = new { DateTime = createAppointmentDto.DateTime };
        //        var slotId = await _dapperRepository.QuerySingleOrDefaultAsync<int>(getSlotSql, slotParameters);

        //        if (slotId == 0)
        //        {
        //            throw new Exception("No available slot found for the specified date and time.");
        //        }

        //        // Step 2: Insert the appointment with the retrieved SlotID
        //        var insertAppointmentSql = @"
        //INSERT INTO [dbo].[Appointments] (DateTime, CreatedDate, LastUpdatedDate, SlotID)
        //VALUES (@DateTime, @CreatedDate, @LastUpdatedDate, @SlotID);
        //SELECT CAST(SCOPE_IDENTITY() as int);";

        //        var appointmentParameters = new
        //        {
        //            DateTime = createAppointmentDto.DateTime,
        //            CreatedDate = DateTime.UtcNow,
        //            LastUpdatedDate = DateTime.UtcNow,
        //            SlotID = slotId
        //        };

        //        var appointmentId = await _dapperRepository.QuerySingleAsync<int>(insertAppointmentSql, appointmentParameters);

        //        // Optionally, update the slot's availability if required
        //        var updateSlotSql = @"
        //UPDATE [dbo].[Slots]
        //SET [Availability] = 'Booked'
        //WHERE [SlotID] = @SlotID";

        //        await _dapperRepository.ExecuteAsync(updateSlotSql, new { SlotID = slotId });

        //        return appointmentId;
        //    }


        //public async Task<int> CreateAppointmentAsync(Appointment appointment)
        //{
        //    var sql = @"
        //        INSERT INTO Appointments (DateTime, CreatedDate, LastUpdatedDate) 
        //        VALUES (@DateTime, @CreatedDate, @LastUpdatedDate);
        //        SELECT CAST(SCOPE_IDENTITY() as int)";

        //    appointment.CreatedDate = DateTime.UtcNow;
        //    appointment.LastUpdatedDate = DateTime.UtcNow;



        //    return await _dapperRepository.ExecuteAsync(sql, appointment);
        //}


        public async Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync()
        {
            var sql = "SELECT * FROM Appointments";
            return await _dapperRepository.GetAllAsync<AppointmentDTO>(sql);
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            var sql = "SELECT * FROM Appointments WHERE AppointmentID = @Id";
            return await _dapperRepository.GetAsync<Appointment>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ReadAppointmentDTO>> GetAppointmentsBySlotCodeNameAsync(string slotCodeName)
        {
            var sql = @"
        SELECT 
            s.DateTime AS SlotDateTime
        FROM 
            [dbo].[Appointments] a
        JOIN 
            [dbo].[Slots] s ON a.SlotID = s.SlotID
        WHERE 
            s.SlotCodeName = @SlotCodeName AND s.Availability = 'Booked'";
            

            var parameters = new { SlotCodeName = slotCodeName };
            var rawAppointments = await _dapperRepository.GetAllAsync<DateTime>(sql, parameters);

            //// Log the raw results
            //foreach (var rawAppointment in rawAppointments)
            //{
            //    Console.WriteLine($"Raw Appointment: {rawAppointment}");
            //}

            // Map the results to ReadAppointmentDTO
            var mappedAppointments = rawAppointments.Select(appointment => new ReadAppointmentDTO
            {
                SlotDateTime = appointment
            }).ToList();

            //// Log the mapped results
            //foreach (var mappedAppointment in mappedAppointments)
            //{
            //    Console.WriteLine($"Mapped Appointment: {mappedAppointment.SlotDateTime}");
            //}

            return mappedAppointments;
        }

        //public async Task<int> UpdateAppointmentToBookedAsync(string slotCodeName, DateTime appointmentDateTime)
        //{
        //    // Find the slot corresponding to the slot code name and appointment date time
        //    var getSlotSql = @"
        //SELECT TOP 1 [SlotID], [Availability]
        //FROM [dbo].[Slots]
        //WHERE [SlotCodeName] = @SlotCodeName
        //AND [DateTime] = @DateTime
        //ORDER BY [SlotID]";

        //    var slotParameters = new { SlotCodeName = slotCodeName, DateTime = appointmentDateTime };
        //    var slot = await _dapperRepository.QuerySingleOrDefaultAsync<(int SlotID, string Availability)>(getSlotSql, slotParameters);

        //    if (slot.SlotID == 0)
        //    {
        //        throw new Exception("No slot found for the specified slot code name and appointment date time.");
        //    }

        //    if (slot.Availability == "Booked")
        //    {
        //        throw new Exception("The slot is already booked.");
        //    }

        //    // Update the current slot's availability to Booked
        //    var updateSlotSql = @"
        //UPDATE [dbo].[Slots]
        //SET [Availability] = 'Booked'
        //WHERE [SlotID] = @SlotID";

        //    await _dapperRepository.ExecuteAsync(updateSlotSql, new { SlotID = slot.SlotID });

        //    // Find and update the previous slot back to available if it's not already booked
        //    var updatePreviousSlotSql = @"
        //UPDATE [dbo].[Slots]
        //SET [Availability] = 'Available'
        //WHERE [SlotID] = (
        //    SELECT TOP 1 [SlotID]
        //    FROM [dbo].[Slots]
        //    WHERE [SlotID] < @SlotID
        //    ORDER BY [SlotID] DESC
        //) AND [Availability] = 'Available'";

        //    await _dapperRepository.ExecuteAsync(updatePreviousSlotSql, new { SlotID = slot.SlotID });

        //    return slot.SlotID;
        //}

        //public async Task<int> UpdateAppointmentToBookedAsync(string slotCodeName, DateTime newAppointmentDateTime)
        //{
        //    // Step 1: Find the first available slot irrespective of SlotCodeName
        //    var getAvailableSlotSql = @"
        //SELECT TOP 1 [SlotID], [Availability], [SlotCodeName], [DateTime]
        //FROM [dbo].[Slots]
        //WHERE [Availability] = 'Available'
        //ORDER BY [SlotID]";

        //    var availableSlot = await _dapperRepository.QuerySingleOrDefaultAsync<(int SlotID, string Availability, string SlotCodeName, DateTime DateTime)>(getAvailableSlotSql);

        //    if (availableSlot.SlotID == 0)
        //    {
        //        throw new Exception("No available slot found.");
        //    }

        //    // Step 2: Update the slot to the new date and time, and change its availability to "Booked"
        //    var updateSlotSql = @"
        //UPDATE [dbo].[Slots]
        //SET [DateTime] = @NewDateTime, [Availability] = 'Booked', [SlotCodeName] = @SlotCodeName
        //WHERE [SlotID] = @SlotID";

        //    var updateSlotParameters = new
        //    {
        //        SlotID = availableSlot.SlotID,
        //        NewDateTime = newAppointmentDateTime,
        //        SlotCodeName = slotCodeName
        //    };

        //    await _dapperRepository.ExecuteAsync(updateSlotSql, updateSlotParameters);

        //    // Step 3: Optionally, find the previous slot and update it back to available if it's not booked again
        //    var updatePreviousSlotSql = @"
        //UPDATE [dbo].[Slots]
        //SET [Availability] = 'Available'
        //WHERE [SlotID] = (
        //    SELECT TOP 1 [SlotID]
        //    FROM [dbo].[Slots]
        //    WHERE [SlotID] < @SlotID AND [Availability] = 'Booked'
        //    ORDER BY [SlotID] DESC
        //)";

        //    await _dapperRepository.ExecuteAsync(updatePreviousSlotSql, new { SlotID = availableSlot.SlotID });

        //    return availableSlot.SlotID;
        //}
        public async Task<IEnumerable<Slot>> GetAvailableSlotsAsync()
        {
            var sql = @"
        SELECT [SlotID], [SlotCodeName], [DateTime], [Availability]
        FROM [dbo].[Slots]
        WHERE [Availability] = 'Available'
        ORDER BY [DateTime]";

            return await _dapperRepository.GetAllAsync<Slot>(sql);
        }

        public async Task<IEnumerable<Slot>> GetBookedSlotsByCodeNameAsync(string slotCodeName)
        {
            var sql = @"
        SELECT [SlotID], [SlotCodeName], [DateTime], [Availability]
        FROM [dbo].[Slots]
        WHERE [SlotCodeName] = @SlotCodeName AND [Availability] = 'Booked'
        ORDER BY [DateTime]";

            return await _dapperRepository.GetAllAsync<Slot>(sql, new { SlotCodeName = slotCodeName });
        }

        public async Task<int> UpdateSlotToBookedAsync(int slotId, DateTime newDateTime)
        {
            var sql = @"
        UPDATE [dbo].[Slots]
        SET [DateTime] = @NewDateTime, [Availability] = 'Booked'
        WHERE [SlotID] = @SlotID";

            return await _dapperRepository.ExecuteAsync(sql, new { SlotID = slotId, NewDateTime = newDateTime });
        }




        public async Task<int> UpdateSlotToAvailableAsync(int slotId)
        {
            var sql = @"
        UPDATE [dbo].[Slots]
        SET [Availability] = 'Available'
        WHERE [SlotID] = @SlotID";

            return await _dapperRepository.ExecuteAsync(sql, new { SlotID = slotId });
        }


        public async Task CancelAppointmentAsync(CancelAppointmentDTO dto)
        {
            // Find the booked slot for the given SlotCodeName and AppointmentDateTime
            var getSlotSql = @"
                SELECT TOP 1 [SlotID], [Availability]
                FROM [dbo].[Slots]
                WHERE [SlotCodeName] = @SlotCodeName
                AND [DateTime] = @DateTime
                AND [Availability] = 'Booked'
                ORDER BY [SlotID]";

            var slotParameters = new { SlotCodeName = dto.SlotCodeName, DateTime = dto.AppointmentDateTime };
            var slot = await _dapperRepository.QuerySingleOrDefaultAsync<(int SlotID, string Availability)>(getSlotSql, slotParameters);

            if (slot.SlotID == 0)
            {
                throw new Exception("No booked slot found for the specified slot code name and appointment date time.");
            }

            // Update the slot's availability to 'Available'
            var updateSlotSql = @"
                UPDATE [dbo].[Slots]
                SET [Availability] = 'Available'
                WHERE [SlotID] = @SlotID";

            await _dapperRepository.ExecuteAsync(updateSlotSql, new { SlotID = slot.SlotID });
        }

        


    }
}
