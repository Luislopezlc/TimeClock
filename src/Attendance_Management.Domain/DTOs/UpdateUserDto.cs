using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [JsonIgnore]
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        [Required]
        public bool? IsActive { get; set; } = true;
        [Required]
        public string EmployeeCode { get; set; }
        public int AreasUserId { get; set; }
        [Required]
        public string AreaCode { get; set; }
        [Required]
        public string PositionCode { get; set; }
        [Required]
        public bool IsLeaderArea { get; set; }

    }
}
