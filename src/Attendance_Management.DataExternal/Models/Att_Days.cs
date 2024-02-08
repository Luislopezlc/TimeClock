using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;

namespace Attendance_Management.DataExternal.Models
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
