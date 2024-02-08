using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.External.Models
{
    public class Personnel_department
    {
        public int id { get; set; }
        public string dept_code { get; set; }
        public string dept_name { get; set; }
        public bool is_default { get; set; }
        public int company_id { get; set; }
        public int? parent_dept_id { get; set; }
    }
}
