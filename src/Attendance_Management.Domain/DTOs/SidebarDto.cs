using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class SidebarDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public bool IsTitle { get; set; }
        public int Priority { get; set; }
        public List<SidebarSecondaryDto> SidebarSecondaries { get; set; }
    }
}
