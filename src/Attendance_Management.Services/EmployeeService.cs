using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.External.Models;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static Humanizer.On;

namespace Attendance_Management.Services
{
	public class EmployeeService : IEmployeeService
	{
		private readonly IEmployeeRepository employeeRepository;
        private readonly ISchedulesService schedulesService;
        private readonly ILogger<EmployeeService> logger;

        public EmployeeService(IEmployeeRepository employeeRepository,ISchedulesService schedulesService, ILogger<EmployeeService> logger)
		{
			this.employeeRepository = employeeRepository;
            this.schedulesService = schedulesService;
            this.logger = logger;
        }

        public async Task<ResponseDto> GetEmployee(string empCode)
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.employeeRepository.GetEmployee(empCode);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            return response;
        }

        public async Task<ResponseDto> GetEmployees()
		{
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.employeeRepository.GetEmployees();
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            return response;
			
		}

        public async Task<ResponseDto> GetEmployeeNameWithEmpCode(string empCode)
        {
            var response = new ResponseDto();
            try
            {
                var employee = await this.GetEmployee(empCode);
                if (!employee.Issuccessful)
                {
                    return employee;
                }

                var employeeDto = employee.Payload as Personnel_employee;

                response.Payload = $"{employeeDto.emp_code} - {employeeDto.first_name} {employeeDto.last_name}";
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
           
            return response;
        }

        public async Task<ResponseDto> GetPositionNameOfEmployee(string empCode)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.employeeRepository.GetPositionNameOfEmployee(empCode);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            
            return response;
        }

        public async Task<ResponseDto> GetUpcomingEmployees()
        {
            var response = new ResponseDto();
            try
            {
                var schedulesResponse = await this.schedulesService.GetSchedules();
                if (!schedulesResponse.Issuccessful)
                {
                    return schedulesResponse;
                }
                var employeeResponse = await this.GetEmployees();
                if (!employeeResponse.Issuccessful)
                {
                    return employeeResponse;
                }

                var employees = employeeResponse.Payload as List<EmployeeDto>;
                var schedules = schedulesResponse.Payload as List<Personnel_Schedules>;
                var upcomingEmployees = new List<EmployeeDto>();
                var today = DateTime.Today.ToString("dd-MM-yyyy");

                foreach (var employee in employees)
                {
                    var schedule = schedules.FirstOrDefault(s => s.EmployeeCode == employee.EmployeeCode
                                                            && s.DayId == (int)Convert.ToDateTime(today).DayOfWeek);

                    if (schedule != null)
                    {
                        var timeUntilStart = Convert.ToDateTime($"{today} {schedule.CheckInTime}") - DateTime.Now;
                        var threshold = TimeSpan.FromMinutes(480);

                        if (timeUntilStart <= threshold && timeUntilStart >= TimeSpan.Zero)
                        {
                            var emp = new EmployeeDto()
                            {
                                FirstName = employee.FirstName,
                                LastName = employee.LastName,
                                Email = employee.Email,
                                EmployeeCode = employee.EmployeeCode,
                                Schedules = new List<Personnel_Schedules>() { schedule },
                            };
                            upcomingEmployees.Add(emp);
                        }
                    }
                }
                response.Payload = upcomingEmployees;
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            
            return response;
        }

        public async Task<ResponseDto> AddEmployee(EmployeeDto request)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.employeeRepository.AddEmployee(request);
                response.Issuccessful = true;

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            return response;
        }

        public async Task<ResponseDto> GetFirstNameEmployee(string employeeCode)
        {
            var response = new ResponseDto();
            try
            {
                var responseEmployee = await this.GetEmployee(employeeCode);
                var employee = responseEmployee.Payload as Personnel_employee;
                string firstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.first_name.ToLower());
                
                response.Payload = firstName;
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            return response;
        }

        public async Task<ResponseDto> EmployeesPaginatedList(EmployeesPaginatedListWithFiltres request)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.employeeRepository.EmployeesPaginatedList(request);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            return response;
        }

        public async Task<ResponseDto> GetEmployeeSearchBar(string request)
        {
            var response = new ResponseDto();
            try
            {
                var payload = new List<string>();
                var employees = await this.employeeRepository.GetEmployeeSearchBar(request);
                if (char.IsLetter(request[0]))
                {
                    payload = employees.OrderByDescending(p => p.FirstName.StartsWith(request))

                                                  .Select(e => e.EmployeeCode + " - " + e.FirstName + " " + e.LastName)
                                                  .ToList();
                }
                else if (char.IsDigit(request[0]))
                {
                    payload = employees.OrderByDescending(p => p.EmployeeCode.StartsWith(request))
                                                 .Select(e => e.EmployeeCode + " - " + e.FirstName + " " + e.LastName)
                                                 .ToList();
                }
                response.Payload = payload;
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
           
            return response;
        }

        public  async Task<ResponseDto> GetEmployeeCodesBySearch(string search)
        {
            var response = new ResponseDto();

            try
            {
                var payload = await this.employeeRepository.GetEmployeeSearchBar(search);
                response.Payload = payload.Select(emp => emp.EmployeeCode).ToList();
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<ResponseDto> GetEmployeeIncidentReport(string empCode)
        {
            var response = new ResponseDto();
            try
            {
                var employee = await this.employeeRepository.GetEmployee(empCode);
                response.Payload = new EmployeeIncidentReportDto()
                {
                    Id = Convert.ToInt32(empCode),
                    FirstName = employee.first_name,
                    LastName = employee.last_name,
                    Job = await this.employeeRepository.GetPositionNameOfEmployee(empCode),
                };
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<ResponseDto> GetAllEmployeCodes(bool IsForIncidentsReport)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.employeeRepository.GetAllEmployeCodes(IsForIncidentsReport);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            return response;
        }

        public async Task<ResponseDto> GetEmployeeDto(string empCode)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.employeeRepository.GetEmployeeDto(empCode);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            return response;
        }

        public async Task<ResponseDto> ExistsEmployee(string empCode)
        {
            var response = new ResponseDto();
            try
            {
                var employee = await this.employeeRepository.GetEmployee(empCode);
                if (employee == null)
                {
                    response.Issuccessful = false;
                    response.Errors = ErrorMessages.AddMessageError("Código de empleado", Messages.EmployeeNotFound);
                    response.Status = 400;
                    return response;
                }
                response.Payload = true;
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            return response;
        }

        public async Task<ResponseDto> GetEmpCodeByName(string Name, string Lastname)
        {
            var response = new ResponseDto();
            try
            {
                var employee = await this.employeeRepository.GetEmpCodeByName(Name, Lastname);
                if(employee != null)
                {
                    response.Payload = employee;
                    response.Issuccessful = true;
                }
                else
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Empleado no encontrado", Messages.UserNotFound);
                }
            } catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            return response;
        }
    }
}
