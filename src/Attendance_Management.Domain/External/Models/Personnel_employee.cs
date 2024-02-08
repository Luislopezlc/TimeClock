using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.External.Models
{
    public class Personnel_employee
    {
        public int id { get; set; }
        public DateTime? create_time { get; set; }
        public string create_user { get; set; }
        public DateTime? change_time { get; set; }
        public string change_user { get; set; }
        public int status { get; set; }
        public int emp_code { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string nickname { get; set; }
        public string passport { get; set; }
        public string driver_license_automobile { get; set; }
        public string driver_license_motorcycle { get; set; }
        public string photo { get; set; }
        public string self_password { get; set; }
        public string device_password { get; set; }
        public int? dev_privilege { get; set; }
        public string card_no { get; set; }
        public string acc_group { get; set; }
        public string acc_timezone { get; set; }
        public string gender { get; set; }
        public DateTime? birthday { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string office_tel { get; set; }
        public string contact_tel { get; set; }
        public string mobile { get; set; }
        public string national_num { get; set; }
        public string payroll_num { get; set; }
        public string internal_emp_num { get; set; }
        public string national { get; set; }
        public string religion { get; set; }
        public string title { get; set; }
        public string enroll_sn { get; set; }
        public string ssn { get; set; }
        public DateTime? update_time { get; set; }
        public DateTime? hire_date { get; set; }
        public int? verify_mode { get; set; }
        public string city { get; set; }
        public bool is_admin { get; set; }
        public int? emp_type { get; set; }
        public bool enable_att { get; set; }
        public bool enable_payroll { get; set; }
        public bool enable_overtime { get; set; }
        public bool enable_holiday { get; set; }
        public bool deleted { get; set; }
        public int? reserved { get; set; }
        public int? del_tag { get; set; }
        public int? app_status { get; set; }
        public int? app_role { get; set; }
        public string email { get; set; }
        public DateTime? last_login { get; set; }
        public bool is_active { get; set; }
        public int? vacation_rule { get; set; }
        public int? company_id { get; set; }
        public int? department_id { get; set; }
        public int? position_id { get; set; }
    }
}
