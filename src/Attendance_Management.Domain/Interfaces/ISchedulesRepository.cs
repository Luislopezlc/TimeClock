using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface ISchedulesRepository   
    {
        Task<Personnel_Schedules> GetSchedule(int Id);
        Task<Att_Days> GetDay(int Id);
        Task<List<Personnel_Schedules>> GetSchedulesByEmployeeCode(string employeeCode);
        Task<List<Personnel_Schedules>> GetSchedules();
        Task<List<Personnel_Schedules>> GetSchedulesWithDay();
        Task<Att_Days> AddSDays(DayDto request);
        Task<List<Personnel_Schedules>> AddSchedule(List<Personnel_SchedulesDto> request);



    }
}
