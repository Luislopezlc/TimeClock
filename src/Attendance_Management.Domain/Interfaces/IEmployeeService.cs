using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;

namespace Attendance_Management.Domain.Interfaces
{
	public interface IEmployeeService
	{
        Task<ResponseDto> GetEmployees();
        Task<ResponseDto> GetEmployee(string empCode);
        Task<ResponseDto> GetEmployeeNameWithEmpCode(string empCode);
        Task<ResponseDto> GetPositionNameOfEmployee(string empCode);
        Task<ResponseDto> GetUpcomingEmployees();
        Task<ResponseDto> GetAllEmployeCodes(bool IsForIncidentsReport);
        Task<ResponseDto> GetEmployeeDto(string empCode);
        Task<ResponseDto> AddEmployee(EmployeeDto request);
        Task<ResponseDto> GetFirstNameEmployee(string employeeCode);
        Task<ResponseDto> EmployeesPaginatedList(EmployeesPaginatedListWithFiltres request);
        Task<ResponseDto> GetEmployeeSearchBar(string request);
        Task<ResponseDto> GetEmployeeCodesBySearch(string search);
        Task<ResponseDto> GetEmployeeIncidentReport(string empCode);

        Task<ResponseDto> ExistsEmployee(string empCode);
        Task<ResponseDto> GetEmpCodeByName(string Name, string Lastname);

    }
}
