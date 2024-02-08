using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Models
{
    public class Auth_Sidebar
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public bool IsTitle { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int Priority { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }
}
