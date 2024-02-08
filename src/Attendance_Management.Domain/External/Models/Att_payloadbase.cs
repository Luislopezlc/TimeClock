using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.External.Models
{
    public class Att_payloadbase
    {
        [Key]
        public string uuid { get; set; }
        public DateTime att_date { get; set; }
        //En la base de datos el campo weekday en esta tabla el valor 0 es el lunes
        public int weekday { get; set; }
        public DateTime check_in { get; set; }
        public DateTime check_out { get; set; }
        public int duration { get; set; }
        public int duty_duration { get; set; }
        public double work_day { get; set; }
        public DateTime? clock_in { get; set; }
        public DateTime? clock_out { get; set; }
        public int total_time { get; set; }
        public int duty_worked { get; set; }
        public int actual_worked { get; set; }
        public int unscheduled { get; set; }
        public int remaining { get; set; }
        public int total_worked { get; set; }
        public int late { get; set; }
        public int early_leave { get; set; }
        [Column("short")]
        public int _short { get; set; }
        public int absent { get; set; }
        public int leave { get; set; }
        public string exception { get; set; }
        public int day_off { get; set; }
        public string break_time_id { get; set; }
        public int emp_id { get; set; }
        public string overtime_id { get; set; }
        public int timetable_id { get; set; }
        public int? trans_in_id { get; set; }
        public int? trans_out_id { get; set; }
    }
}
