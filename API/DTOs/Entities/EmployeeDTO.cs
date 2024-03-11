using Core.Enums;
using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Entities
{
    public class EmployeeDTO : BaseEntityDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]

        public string PhoneNumber { get; set; }
        [Required]

        public GraduationStatus GraduationStatus { get; set; }

        public string ImageUrI { get; set; }

        public string CreatedBy { get; set; }
       
        public IFormFile Photo { get; set; }
    }
}
