using AutoMapper;
using Slot_Service.Database.IDapperRepositorys;
using Slot_Service.Models;
using Slot_Service.Models.DTO;
using Slot_Service.Repository.IRepository;
using Slot_Service.Service.Service;

namespace Slot_Service.Repository.Repository
{
    //public class SlotRepository : ISlotRepository
    //{
    //    private readonly IDapperRepository _dapperRepository;
    //    private readonly AuthService _authService;
    //    private readonly IMapper _mapper;
    //    public SlotRepository(IDapperRepository dapperRepository, IMapper mapper,AuthService authService)
    //    {
    //        _dapperRepository = dapperRepository;
    //        _mapper = mapper;
    //        _authService = authService;
    //    }
    //    public async Task<int> CreateSlotAsync(CreateSlotDTO createSlotDto)
    //    {
    //        var userSlotCodeName=_authService.GetCurrentUserId();
    //        var sql = @"
    //        INSERT INTO Slots (Availability, DateTime, CreatedDate, LastUpdatedDate, SlotCodeName) 
    //        VALUES (@Availability, @DateTime, @CreatedDate, @LastUpdatedDate, @SlotCodeName);
    //        SELECT CAST(SCOPE_IDENTITY() as int)";

    //        var createSlot = _mapper.Map<Slot>(createSlotDto);
    //        createSlot.CreatedDate = DateTime.UtcNow;
    //        createSlot.LastUpdatedDate = DateTime.UtcNow;
    //        createSlot.availability = "Available";
    //        createSlot.SlotCodeName= userSlotCodeName;

    //        return await _dapperRepository.ExecuteAsync(sql, createSlot);
    //    }

    //    public async Task<IEnumerable<SlotDTO>> GetAllSlotsAsync()
    //    {
    //        var sql = "SELECT * FROM Slots";
    //        var slots = await _dapperRepository.GetAllAsync<Slot>(sql);
    //        return _mapper.Map<IEnumerable<SlotDTO>>(slots);
    //    }

    //    public async Task<SlotDTO> GetSlotByIdAsync(int id)
    //    {
    //        var sql = "SELECT * FROM Slots WHERE SlotID = @Id";
    //        var slot = await _dapperRepository.GetAsync<Slot>(sql, new { Id = id });
    //        return _mapper.Map<SlotDTO>(slot);
    //    }
    //    public async Task<IEnumerable<SlotDTO>> GetSlotByCodeAsync()
    //    {
    //        var userSlotCodeName = _authService.GetCurrentUserId();
    //        var sql = "SELECT * FROM Slots WHERE SlotCodeName = @SlotCodeName";
    //        var slot = await _dapperRepository.GetAllAsync<Slot>(sql, new { SlotCodeName= userSlotCodeName });
    //        return _mapper.Map<IEnumerable<SlotDTO>>(slot);
    //    }

    //    public async Task<bool> UpdateSlotAsync(string SlotCodeName, int id, CreateSlotDTO updateSlotDto)
    //    {
    //        var sql = @"
    //        UPDATE Slots SET 
    //            Availability = @Availability, 
    //            DateTime = @DateTime, 
    //            LastUpdatedDate = @LastUpdatedDate 
    //        WHERE SlotCodeName = @SlotCodeName AND SlotID = @SlotID";

    //        var updateSlot = _mapper.Map<Slot>(updateSlotDto);
    //        updateSlot.LastUpdatedDate = DateTime.UtcNow;
    //        updateSlot.SlotID = id;
    //        updateSlot.SlotCodeName = SlotCodeName;
    //        updateSlot.availability = "Availability";

    //        var rowsAffected = await _dapperRepository.ExecuteAsync(sql, new
    //        {
    //            SlotID = updateSlot.SlotID,
    //            Availability = updateSlot.availability,
    //            DateTime = updateSlot.DateTime,
    //            LastUpdatedDate = updateSlot.LastUpdatedDate,
    //            SlotCodeName = updateSlot.SlotCodeName
    //        });
    //        return rowsAffected > 0;
    //    }


    //    public async Task<bool> DeleteSlotAsync(string slotCodeName, int id)
    //    {
    //        var sql = "DELETE FROM Slots WHERE SlotID = @Id AND SlotCodeName = @SlotCodeName";
    //        var rowsAffected = await _dapperRepository.ExecuteAsync(sql, new { Id = id, SlotCodeName = slotCodeName });
    //        return rowsAffected > 0;
    //    }


    //}

    public class SlotRepository : ISlotRepository
    {
        private readonly IDapperRepository _dapperRepository;
        private readonly IMapper _mapper;

        public SlotRepository(IDapperRepository dapperRepository, IMapper mapper)
        {
            _dapperRepository = dapperRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateSlotAsync(Slot slot)
        {
            var sql = @"
            INSERT INTO Slots (Availability, DateTime, CreatedDate, LastUpdatedDate, SlotCodeName) 
            VALUES (@Availability, @DateTime, @CreatedDate, @LastUpdatedDate, @SlotCodeName);
            SELECT CAST(SCOPE_IDENTITY() as int)";

            return await _dapperRepository.ExecuteAsync(sql, slot);
        }

        public async Task<IEnumerable<SlotDTO>> GetSlotByCodeAsync(string slotCodeName)
        {
            var sql = "SELECT * FROM Slots WHERE SlotCodeName = @SlotCodeName";
            var slots = await _dapperRepository.GetAllAsync<Slot>(sql, new { SlotCodeName = slotCodeName });
            foreach (var slot in slots)
            {
                Console.WriteLine($"{slot.SlotCodeName}{slot.DateTime}{slot.availability}{slot.SlotID}");
            }
            return _mapper.Map<IEnumerable<SlotDTO>>(slots);
        }

        public async Task<SlotDTO> GetSlotByIdAndCodeAsync(int id, string slotCodeName)
        {
            var sql = "SELECT * FROM Slots WHERE SlotID = @Id AND SlotCodeName = @SlotCodeName";
            var slot = await _dapperRepository.GetAsync<Slot>(sql, new { Id = id, SlotCodeName = slotCodeName });
            return _mapper.Map<SlotDTO>(slot);
        }

        public async Task<bool> UpdateSlotAsync(string slotCodeName, int id, CreateSlotDTO updateSlotDto)
        {
            var sql = @"
            UPDATE Slots SET 
                
                DateTime = @DateTime 
                
            WHERE SlotID = @SlotID AND SlotCodeName = @SlotCodeName";

            var slot = _mapper.Map<Slot>(updateSlotDto);
            slot.LastUpdatedDate = DateTime.UtcNow;
            slot.SlotID = id;
            slot.SlotCodeName = slotCodeName;
            slot.availability = "Available";


            return await _dapperRepository.ExecuteAsync(sql, slot) > 0;
        }

        public async Task<bool> DeleteSlotAsync(string slotCodeName, int id)
        {
            var sql = "DELETE FROM Slots WHERE SlotID = @Id AND SlotCodeName = @SlotCodeName";
            return await _dapperRepository.ExecuteAsync(sql, new { Id = id, SlotCodeName = slotCodeName }) > 0;
        }
    }
}
