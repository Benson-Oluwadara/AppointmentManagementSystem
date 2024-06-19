using AutoMapper;
using PatientService.Models;
using PatientService.Models.DTO;

namespace PatientService.Mapping
{
    //public class MappingConfig:Profile
    //{

    //    CreateMap<Patient, PatientDTO>().ReverseMap();
    //}

    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Patient, CreatePatientDTO>().ReverseMap();

        }

    }
}
