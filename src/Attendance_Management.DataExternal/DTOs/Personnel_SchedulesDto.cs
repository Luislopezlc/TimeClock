using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.DataExternal.DTOs
{
    public class Personnel_SchedulesDto
    {
        [Required]
        public string EmployeeCode { get; set; }
        [Required]
        public string CheckInTime { get; set; }
        [Required]
        public string CheckOutTime { get; set; }
        public int DayId { get; set; }
    }
}
