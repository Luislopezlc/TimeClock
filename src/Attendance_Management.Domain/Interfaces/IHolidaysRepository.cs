using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface IHolidaysRepository
    {
        Task<HolidaysPaginatedListDto> GetListPaginatedHolidays(HolidaysPaginatedListDto request);
        Task<List<Att_HolidaysDto>> GetHolidays(bool? isActive);
        Task<Att_Holidays> GetHoliday(int Id);
        Task<GetHolidaysDto> GetHolidayDto(int Id);
        Task<Att_Holidays> GetHolidayPartialByEmpCode(string day, string empcode);
        Task<Att_Holidays> GetHoliday(DateTime day);
        Task<List<Att_HolidaysEmployees>> GetHolidaysEmployees();
        Task<Att_HolidaysEmployees> GetHolidayEmployee(int Id);
        Task<Att_HolidaysEmployees> GetHolidaysEmployeesWithHolidayId(int Id);
        Task<Att_HolidaysEmployees> GetHolidaysEmployeesWithDepartmentId(int Id);
        Task<Att_Holidays> AddHolidays(Att_HolidaysDto request, Att_Holidays entityToUpdate);
        Task<Att_HolidaysEmployees> AddHolidaysEmployees(Att_HolidaysEmployeesDto request);
        Task<bool> DeleteHoliday(int Id);
    }
}
