using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.DataExternal.DTOs
{
    public class TransactionsByRangeDto
    {
        public List<string> EmployeeCodes { get; set; }
        public string InitialDate { get; set; }
        public string EndDate { get; set; }
    }
}
