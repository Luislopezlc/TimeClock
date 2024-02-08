using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface IHolidaysService
    {
        Task<ResponseDto> GetListPaginatedHolidays(HolidaysPaginatedListDto request);
        Task<ResponseDto> GetHolidays(bool? isActive);
        Task<ResponseDto> GetHoliday(int Id);
        Task<ResponseDto> GetHoliday(string day);
        Task<ResponseDto> GetHolidayDto(int Id);
        Task<ResponseDto> DeleteHoliday(int Id);
        Task<ResponseDto> GetHolidaysEmployees();
        Task<ResponseDto> GetHolidayEmployee(int Id);
        Task<ResponseDto> GetHolidaysEmployeesWithHolidayId(int Id);
        Task<ResponseDto> GetHolidaysEmployeesWithDepartmentId(int Id);
        Task<ResponseDto> AddHolidays(AddHolidayDto request);
        Task<ResponseDto> AddHolidaysEmployees(Att_HolidaysEmployeesDto request);
        Task<ResponseDto> GetHolidayPartialByEmpCode(string day, string empcode);
    }
}
