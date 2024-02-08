using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class HolidaysEmployeesDto
    {
        [Required(ErrorMessage = "El campo 'HolidayId' es requerido")]
        public int HolidaysId { get; set; }
        [Required(ErrorMessage = "El campo 'DepartmentId' es requerido")]
        public int DepartmentId { get; set; }
    }
}
