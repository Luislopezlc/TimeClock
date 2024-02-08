using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class UserCredentialsGoogleDto
    {
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
