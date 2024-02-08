using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Job { get; set; }
        public string FullName
        {
            get
            {
                return $"{EmployeeCode} - {FirstName} {LastName}";
            }
            set
            {
                FullName = value;
            }
        }
        public List<Personnel_Schedules> Schedules { get; set; } = new List<Personnel_Schedules>();
    }
}
