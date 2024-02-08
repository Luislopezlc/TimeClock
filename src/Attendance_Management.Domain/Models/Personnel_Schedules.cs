using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Management.Domain.Models
{
    public class Personnel_Schedules
    {
        [Key]  
        public int Id { get; set; }
        [Required]
        public string EmployeeCode { get; set; }
        [Required]
        public string CheckInTime { get; set; }
        [Required]
        public string CheckOutTime { get; set;}
        public int DayId { get; set; }
        [ForeignKey("DayId")]
        public Att_Days Day { get; set; }
    }
}
