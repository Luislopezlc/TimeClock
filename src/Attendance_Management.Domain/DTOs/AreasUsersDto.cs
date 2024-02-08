using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class AreasUsersDto
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public string Area { get; set; }
        public string Position { get; set; }
        public bool IsLeader { get; set; }
    }
}
