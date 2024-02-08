using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Models
{
    public class Att_HolidaysEmployees
    {
        [Key]
        public int Id { get; set; }
        
        public int HolidaysId { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("HolidaysId")]
        public Att_Holidays Holidays { get; set; }
        [ForeignKey("DepartmentId")]
        public Deparment Department { get; set; }
    }
}
