using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class Att_HolidaysDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo 'Día' es requerido")]
        public DateTime Day { get; set; }
        [Required(ErrorMessage = "El campo 'IsPartial' es requerido")]
        public bool IsPartial { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string> Departments { get; set; } = new List<string>();
    }
}
