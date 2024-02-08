using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class IncidentToday
    {
        public int Id { get; set; }
        public string Employee { get; set; }
        public string Type { get; set; }
    }
}
