using Slot_Service.Models.DTO;
using Slot_Service.Service.IService;
using AutoMapper;
using Slot_Service.Repository.IRepository;
using Slot_Service.Models;

namespace Slot_Service.Service.Service
{
    //public class SlotService : ISlotService
    //{
    //    private readonly ISlotRepository _slotRepository;
    //    private readonly IMapper _mapper;

    //    public SlotService(ISlotRepository slotRepository, IMapper mapper)
    //    {
    //        _slotRepository = slotRepository;
    //        _mapper = mapper;
    //    }

    //    public async Task<SlotDTO> CreateSlotAsync(CreateSlotDTO createSlotDto)
    //    {
    //        int slotId = await _slotRepository.CreateSlotAsync(createSlotDto);
    //        var slot = _mapper.Map<Slot>(createSlotDto);
    //        slot.SlotID = slotId;
    //        return _mapper.Map<SlotDTO>(slot);

    //    }
    //    public async Task<IEnumerable<SlotDTO>> GetAllSlotsAsync()
    //    {
    //        return await _slotRepository.GetAllSlotsAsync();
    //    }

    //    public async Task<SlotDTO> GetSlotByIdAsync(int id)
    //    {
    //        return await _slotRepository.GetSlotByIdAsync(id);
    //    }

    //    public async Task<bool> UpdateSlotAsync(string SlotCodeName, int id, CreateSlotDTO updateSlotDto)
    //    {
    //        return await _slotRepository.UpdateSlotAsync(SlotCodeName, id, updateSlotDto);
    //    }

    //    public async Task<bool> DeleteSlotAsync(string slotCodeName, int id)
    //    {
    //        return await _slotRepository.DeleteSlotAsync(slotCodeName, id);
    //    }
    //    public async Task<IEnumerable<SlotDTO>> GetSlotByCodeAsync()
    //    {
    //        return await _slotRepository.GetSlotByCodeAsync();
    //    }
    //}
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository _slotRepository;
        private readonly IMapper _mapper;

        public SlotService(ISlotRepository slotRepository, IMapper mapper)
        {
            _slotRepository = slotRepository;
            _mapper = mapper;
        }

        public async Task<SlotDTO> CreateSlotAsync(CreateSlotDTO createSlotDto, string slotCodeName)
        {
            var slot = _mapper.Map<Slot>(createSlotDto);
            slot.CreatedDate = DateTime.UtcNow;
            slot.LastUpdatedDate = DateTime.UtcNow;
            slot.availability = "Available";
            slot.SlotCodeName = slotCodeName;

            int slotId = await _slotRepository.CreateSlotAsync(slot);
            slot.SlotID = slotId;
            return _mapper.Map<SlotDTO>(slot);
        }

        public async Task<IEnumerable<SlotDTO>> GetSlotByCodeAsync(string slotCodeName)
        {
            return await _slotRepository.GetSlotByCodeAsync(slotCodeName);
        }

        public async Task<SlotDTO> GetSlotByIdAndCodeAsync(int id, string slotCodeName)
        {
            return await _slotRepository.GetSlotByIdAndCodeAsync(id, slotCodeName);
        }

        public async Task<bool> UpdateSlotAsync(string slotCodeName, int id, CreateSlotDTO updateSlotDto)
        {
            return await _slotRepository.UpdateSlotAsync(slotCodeName, id, updateSlotDto);
        }

        public async Task<bool> DeleteSlotAsync(string slotCodeName, int id)
        {
            return await _slotRepository.DeleteSlotAsync(slotCodeName, id);
        }
    }
}
