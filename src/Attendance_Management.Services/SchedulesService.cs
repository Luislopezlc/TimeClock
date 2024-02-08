using Attendance_Management.Domain.Models;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.DTOs;
using Microsoft.Extensions.Logging;

namespace Attendance_Management.Services
{
    public class SchedulesService : ISchedulesService
    {
        private readonly ISchedulesRepository schedulesRepository;
        private readonly ILogger<SchedulesService> logger;

        public SchedulesService(ISchedulesRepository schedulesRepository, ILogger<SchedulesService> logger)
        {
            this.schedulesRepository = schedulesRepository;
            this.logger = logger;
        }

        public async Task<ResponseDto> AddSchedule(List<Personnel_SchedulesDto> request)
        {
            //creamos el objeto comun que devolveremos a vista
            var response = new ResponseDto();
            try
            {
                //revisamos las reglas 

                //un horario no debe tener una hora de salida menos a su hora de entrada

                var dateShort = DateTime.Now.ToShortDateString();

                //revisamos si hay algun registro que tenga una hora de salida menor o igual asu hora de entrada
                var shorterDepartureTime = request.Where(r => Convert.ToDateTime(dateShort + " " + r.CheckInTime) >=
                 Convert.ToDateTime(dateShort + " " + r.CheckOutTime)).ToList();

                //si existe un registro con una hora de salida menor o igual a su hora de entrada entonces devolvemos un error 
                if (shorterDepartureTime != null && shorterDepartureTime.Any())
                {
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("CheckOutError", Messages.CheckOutError);
                    return response;
                }

                response.Payload = await this.schedulesRepository.AddSchedule(request);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Status = 500;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }
            return response;
        }

        public async Task<ResponseDto> AddSDays(DayDto request)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.schedulesRepository.AddSDays(request);
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

        public async Task<ResponseDto> GetDay(int Id)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.schedulesRepository.GetDay(Id);
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

        public async Task<ResponseDto> GetSchedule(int Id)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.schedulesRepository.GetSchedule(Id);
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

        public async Task<ResponseDto> GetSchedules()
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.schedulesRepository.GetSchedules();
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

        public async Task<ResponseDto> GetSchedulesByEmployeeCode(string employeeCode)
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.schedulesRepository.GetSchedulesByEmployeeCode(employeeCode);
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

        //Este metodo devuelve los horarios haciendo un join con la entity Day
        public async Task<ResponseDto> GetSchedulesWithDay()
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.schedulesRepository.GetSchedulesWithDay();
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
    }
}
