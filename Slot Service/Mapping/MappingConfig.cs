using AutoMapper;
using Slot_Service.Models.DTO;
using Slot_Service.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Slot_Service.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //CreateMap<Models.Slot, Models.DTO.SlotDTO>().ReverseMap();
            //CreateMap<Models.Slot, Models.DTO.CreateSlotDTO>().ReverseMap();
            CreateMap<CreateSlotDTO, Slot>();
                //.ForMember(dest => dest.availability, opt => opt.MapFrom(src => src.Availability.First()))
                //.ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                //.ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore())
                //.ForMember(dest => dest.SlotID, opt => opt.Ignore());

            CreateMap<Slot, SlotDTO>();


        }
    }
}
