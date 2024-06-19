using DoctorService.Models.DTO;
using DoctorService.Models;
using AutoMapper;

namespace DoctorService.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorDTO>().ReverseMap();
        }
    }
}
