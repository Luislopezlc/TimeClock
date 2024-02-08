using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface ITemplateService
    {
        Task<string>GetIncidentsReportEmailTemplate(string filePath);
    }
}
