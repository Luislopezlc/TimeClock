using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Models
{
    public class sidebarRoles
    {
        [Key]
        public int Id { get; set; }
        public int AuthId { get; set; }
        public string RolId { get; set; }
        [ForeignKey("AuthId")]
        public Auth_Sidebar auth_Sidebar { get; set; }
        [ForeignKey("RolId")]
        public IdentityRole identityRole { get; set; }
    }
}
