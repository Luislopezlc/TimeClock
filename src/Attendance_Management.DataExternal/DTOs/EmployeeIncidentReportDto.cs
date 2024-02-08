using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.DataExternal.DTOs
{
    public class EmployeeIncidentReportDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Job { get; set; }
        public int Absences { get; set; }
        public List<string> AbsenceDates { get; set; } = new();
        public int MinorDelays { get; set; }
        public List<string> MinorDelayDates { get; set; } = new();
        public int LongerDelays { get; set; }
        public List<string> LongerDelaysDates { get; set; } = new();
        public int TotalIncidents { get; set; }
    }
}
