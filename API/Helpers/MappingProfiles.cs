using API.DTOs;
using API.DTOs.Entities;
using AutoMapper;
using Core.Models;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<BaseEntity, BaseEntityDTO>();
                     
            CreateMap<Employee, EmployeeDTO>().ForMember(d => d.ImageUrI, o => o.MapFrom<EmployeeImageUrIResolver>());
     
        }
    }
}
