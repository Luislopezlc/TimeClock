using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }
        public string Rol { get; set; } = "Admin";
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public List<AreasUsersDto> AreasUsers { get; set; }
    }
}   
