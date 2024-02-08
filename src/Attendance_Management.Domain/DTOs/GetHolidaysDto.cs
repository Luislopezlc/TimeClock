using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class GetHolidaysDto
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public bool IsPartial { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public bool IsActive { get; set; }
        public List<string> Departments { get; set; } = new List<string>(); //los nombre de los departamentos relacionados al holiday
    }
}
