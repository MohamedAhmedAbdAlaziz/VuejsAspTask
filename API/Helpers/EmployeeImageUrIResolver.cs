using API.DTOs;
using API.DTOs.Entities;
using AutoMapper;
using Core.Models;
 
namespace API.Helpers
{
    public class EmployeeImageUrIResolver : IValueResolver<Employee, EmployeeDTO, string>
    {
        private readonly IConfiguration _config;

        public EmployeeImageUrIResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Employee source, EmployeeDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrI))
            {
                return $"{_config["ApiUrl"]}Images/{source.ImageUrI}";
            }
            return null;

        }

    }

}