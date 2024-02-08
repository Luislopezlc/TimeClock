using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.DataExternal.Models
{
    public class iclock_transaction
    {
        public int id { get; set; }
        public string emp_code { get; set; }
        public DateTime punch_time { get; set; }
        public string punch_state { get; set; }
        public int verify_type { get; set; }
        public string work_code { get; set; }
        public string terminal_sn { get; set; }
        public string terminal_alias { get; set; }
        public string area_alias { get; set; }
        public decimal? longitude { get; set; }
        public decimal? latitude { get; set; }
        public string gps_location { get; set; }
        public string mobile { get; set; }
        public int? source { get; set; }
        public int?  purpose { get; set; }
        public string crc { get; set; }
        public int? is_attendance { get; set; }
        public int? reserved { get; set; }
        public DateTime? upload_time { get; set; }
        public int? sync_status { get; set; }
        public DateTime? sync_time { get; set; }
        public int? emp_id { get; set; }
        public int? terminal_id { get; set; }
        public int? is_mask { get; set; }
        public int? temperature { get; set; }
    }
}
