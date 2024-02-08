using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface IGeneralConfigurationService
    {
        Task<ResponseDto> GetGeneralConfiguration(string ConfigurationName);
        Task<ResponseDto> UpdateGeneralConfiguration(GeneralConfiguration request);

    }
}
