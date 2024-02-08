using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class IncidenctsReportEmailDto
    {
        public string InitialDate { get; set; }
        public string EndDate { get; set; }
        public List<string> EmployeeCodes { get; set; }
    }
}
