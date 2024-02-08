using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.External.Models
{
    public class Personnel_position
    {
        public int id { get; set; }
        public string position_code { get; set; }
        public string position_name { get; set; }
        public bool is_default { get; set; }
        public int company_id { get; set; }
        public int? parent_position_id { get; set; }
    }
}
