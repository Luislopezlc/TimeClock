using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface IGeneralConfigurationRepository
    {
        Task<GeneralConfiguration> GetGeneralConfiguration(string ConfigurationName);
        Task<bool> ExistsGeneralConfiguration(string name);
        Task<GeneralConfiguration> UpdateGeneralConfiguration(GeneralConfiguration request);
    }
}
