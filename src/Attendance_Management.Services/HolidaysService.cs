using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Services
{
    public class HolidaysService : IHolidaysService
    {
        private readonly IHolidaysRepository holidaysRepository;
        private readonly IBusinessStructureRepository businessStructureRepository;
        private readonly ILogger<HolidaysService> logger;

        public HolidaysService(IHolidaysRepository holidaysRepository, ILogger<HolidaysService> logger, IBusinessStructureRepository businessStructureRepository)
        {
            this.holidaysRepository = holidaysRepository;
            this.businessStructureRepository= businessStructureRepository;
            this.logger = logger;
        }
        public async Task<ResponseDto> AddHolidays(AddHolidayDto request)
        {
            var response = new ResponseDto();
            

            try
            {
                if (request.Departments != null && request.Departments.Any())
                {
                    foreach (var dep in request.Departments)
                    {
                        var exists = await this.businessStructureRepository.GetDeparment(dep);
                        if (exists == null)
                        {
                            response.Issuccessful = false;
                            response.Errors = ErrorMessages.AddMessageError("Departamento", Messages.DepartmentNotFound + " " + dep);
                            response.Status = 400;
                        }
                    }
                }

                var att_holidayDto = new Att_HolidaysDto()
                {
                    Id = request.Id,
                    Day = Convert.ToDateTime(request.Day),
                    IsActive = request.IsActive,
                    IsPartial = request.IsPartial,
                    CheckIn = (!string.IsNullOrEmpty(request.CheckIn)) ? Convert.ToDateTime(request.Day + " " + request.CheckIn) : null,
                    CheckOut = (!string.IsNullOrEmpty(request.CheckOut)) ? Convert.ToDateTime(request.Day + " " + request.CheckOut) : null,
                    Departments = request.Departments,
                };

                var holiday = await this.holidaysRepository.GetHoliday(request.Id);
                
                    response.Payload = await this.holidaysRepository.AddHolidays(att_holidayDto, holiday);
                    response.Issuccessful = true;
                    response.Status = 200;
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

        public async Task<ResponseDto> AddHolidaysEmployees(Att_HolidaysEmployeesDto request)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.holidaysRepository.AddHolidaysEmployees(request);
                response.Issuccessful = true;
                response.Status = 200;
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

        public async Task<ResponseDto> DeleteHoliday(int Id)
        {
            var response = new ResponseDto();
            try
            {
                var holiday =  await this.holidaysRepository.GetHoliday(Id);
                if (holiday == null)
                {
                    response.Issuccessful = false;
                    response.Errors = ErrorMessages.AddMessageError("Holiday", Messages.HolidayNotFound);
                    response.Status = 400;
                    return response;
                }
                var deleted = await this.holidaysRepository.DeleteHoliday(Id);
                response.Payload = deleted;
                response.Issuccessful = true;
                response.Status = 200;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }
            return response;
        }

        public async Task<ResponseDto> GetHoliday(int Id)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.holidaysRepository.GetHoliday(Id);
                response.Issuccessful = true;
                response.Status = 200;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }
            return response;
        }

        public async Task<ResponseDto> GetHoliday(string day)
        {
            var response = new ResponseDto();


            try
            {
                DateTime date = Convert.ToDateTime(day);
                response.Payload = await this.holidaysRepository.GetHoliday(date);
                response.Issuccessful = true;
                response.Status = 200;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }

            return response;
        }

        public async Task<ResponseDto> GetHolidayDto(int Id)
        {
            var response = new ResponseDto();

            var exists = await this.holidaysRepository.GetHoliday(Id);
            if (exists == null)
            {
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError("Holiday", Messages.HolidayNotFound);
                response.Status = 400;
                return response;
            }

            try
            {
                response.Payload = await this.holidaysRepository.GetHolidayDto(Id);
                response.Issuccessful = true;
                response.Status = 200;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }

            return response;
        }

        public async Task<ResponseDto> GetHolidayEmployee(int Id)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.holidaysRepository.GetHolidayEmployee(Id);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }
            return response;
        }

        public async Task<ResponseDto> GetHolidayPartialByEmpCode(string day, string empcode)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.holidaysRepository.GetHolidayPartialByEmpCode(day, empcode);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }
            return response;
        }

        public async Task<ResponseDto> GetHolidays(bool? isActive)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.holidaysRepository.GetHolidays(isActive);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }
            return response;
        }

        public async Task<ResponseDto> GetHolidaysEmployees()
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.holidaysRepository.GetHolidaysEmployees();
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }
            return response;
        }

        public async Task<ResponseDto> GetHolidaysEmployeesWithDepartmentId(int Id)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.holidaysRepository.GetHolidaysEmployeesWithDepartmentId(Id);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }
            return response;
        }

        public async Task<ResponseDto> GetHolidaysEmployeesWithHolidayId(int Id)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.holidaysRepository.GetHolidaysEmployeesWithHolidayId(Id);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }
            return response;
        }

        public async Task<ResponseDto> GetListPaginatedHolidays(HolidaysPaginatedListDto request)
        {
            var response = new ResponseDto();   

            try
            {
                var daySearch = new DateTime();
                if (!string.IsNullOrEmpty(request.Search))
                {
                    if (DateTime.TryParse(request.Search, out daySearch))
                    {
                        request.Search = daySearch.ToString();

                    }
                    else
                    {
                        response.Issuccessful = false;
                        response.Errors = ErrorMessages.AddMessageError("Search", Messages.DateTimeFormatInvalid);
                        response.Status = 400;
                        return response;
                    }
                }
                

                response.Payload = await this.holidaysRepository.GetListPaginatedHolidays(request);
                response.Issuccessful = true;
                response.Status = 200;


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
    }
}
