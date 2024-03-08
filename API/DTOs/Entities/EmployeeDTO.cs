using Core.Enums;
using Core.Models;

namespace API.DTOs.Entities
{
    public class EmployeeDTO : BaseEntityDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GraduationStatus GraduationStatus { get; set; }
        public string ImageUrI { get; set; }

        public string CreatedBy { get; set; }
        public User User { get; set; }
        public IFormFile Photo { get; set; }
    }
}
