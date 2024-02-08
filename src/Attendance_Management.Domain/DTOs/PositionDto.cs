using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class PositionDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, StringLength(10)]  
        public string Code { get; set; }
    }
}
