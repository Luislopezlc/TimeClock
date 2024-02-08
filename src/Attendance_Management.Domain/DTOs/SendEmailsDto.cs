using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class SendEmailsDto
    {
        public string ApiKey { get; set; }
        public IncidenctsReportEmailDto IncidenctsReportEmailDto { get; set; }
    }
}
