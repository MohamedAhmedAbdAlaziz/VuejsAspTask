using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public  string Email { get; set; }
        public  string PhoneNumber { get; set; }
        public  GraduationStatus GraduationStatus { get; set; }
        public  string ImageUrI { get; set; }

        public string CreatedBy { get; set; }
        public User User { get; set; }
        


    }
}
