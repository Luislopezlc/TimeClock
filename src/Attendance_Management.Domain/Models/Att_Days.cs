using System.ComponentModel.DataAnnotations;

namespace Attendance_Management.Domain.Models
{
    public class Att_Days
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Value { get; set; }
       
    }
}
