using Attendance_Management.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Attendance_Management.Services
{
    public  class TemplateService : ITemplateService
    {

        public async Task<string> GetIncidentsReportEmailTemplate(string filePath)
        {
            

            if (File.Exists(filePath))
            {
                return await File.ReadAllTextAsync(filePath);
            }   

            return null; // Devuelve null si el archivo no se encuentra.
        }
    }
}
