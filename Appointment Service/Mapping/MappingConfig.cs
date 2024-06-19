using Appointment_Service.Models.DTO;
using Appointment_Service.Models;
using AutoMapper;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Appointment_Service.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
             
            
            CreateMap<Appointment, CreateAppointmentDTO>().ReverseMap();
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();
            CreateMap<Appointment, UpdateAppointmentToBookedDTO>().ReverseMap();


        }
    }
}
