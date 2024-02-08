using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.DataExternal.DTOs
{
    public class IncidentReportDto
    {
        public string InitialDate { get; set; }
        public string EndDate { get; set; }
        public List<EmployeeIncidentReportDto> Employees { get; set; } = new();
    }
}
