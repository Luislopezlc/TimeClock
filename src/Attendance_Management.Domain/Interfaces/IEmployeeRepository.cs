using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.External.Models;
using static Humanizer.On;

namespace Attendance_Management.Domain.Interfaces
{
	public interface IEmployeeRepository
	{
        Task<List<EmployeeDto>> GetEmployees();
		Task<Personnel_employee> GetEmployee(string empCode);
		//si recibe un true debe de mandar solo los emp_codes de los empleados que tengas horario
		Task<List<string>> GetAllEmployeCodes(bool IsForIncidentsReport);
		Task<string> GetPositionNameOfEmployee(string empCode);
		Task<Personnel_employee> AddEmployee(EmployeeDto request);
		Task<EmployeesPaginatedListDto> EmployeesPaginatedList(EmployeesPaginatedListWithFiltres request);
		Task<List<EmployeeDto>> GetEmployeeSearchBar(string request);
		Task<EmployeeDto> GetEmployeeDto(string empCode);
		Task<int?> GetEmpCodeByName(string Name, string Lastname);


	}
}
