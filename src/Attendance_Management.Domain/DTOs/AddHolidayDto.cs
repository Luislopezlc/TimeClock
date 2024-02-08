using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class AddHolidayDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo 'Día' es requerido")]
        public string Day { get; set; }
        [Required(ErrorMessage = "El campo 'IsPartial' es requerido")]
        public bool IsPartial { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public bool IsActive { get; set; }
        public List<string> Departments { get; set; } = new List<string>();
    }
}
