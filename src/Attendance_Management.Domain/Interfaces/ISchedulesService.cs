using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface ISchedulesService
    {
        Task<ResponseDto> GetSchedule(int Id);
        Task<ResponseDto> AddSchedule(List<Personnel_SchedulesDto> request);
        Task<ResponseDto> GetDay(int Id);
        Task<ResponseDto> GetSchedulesByEmployeeCode(string employeeCode);

        Task<ResponseDto> GetSchedules();
        Task<ResponseDto> GetSchedulesWithDay();
        Task<ResponseDto> AddSDays(DayDto request);
    }
}
