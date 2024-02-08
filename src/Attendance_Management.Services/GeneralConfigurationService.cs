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
    public class GeneralConfigurationService : IGeneralConfigurationService
    {
        private readonly IGeneralConfigurationRepository generalConfigurationRepository;
        private readonly ILogger<GeneralConfigurationService> logger;

        public GeneralConfigurationService(IGeneralConfigurationRepository generalConfigurationRepository, ILogger<GeneralConfigurationService> logger )
        {
            this.generalConfigurationRepository = generalConfigurationRepository;
            this.logger = logger;
        }
        public async Task<ResponseDto> GetGeneralConfiguration(string ConfigurationName)
        {
            var response = new ResponseDto();
            try
            {
                var existsConfiguration = await this.generalConfigurationRepository.ExistsGeneralConfiguration(ConfigurationName);
                if(!existsConfiguration)
                {
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("GeneralConfigurationNotFound", Messages.GeneralConfigurationNotFound);
                    return response;
                }

                response.Payload = await this.generalConfigurationRepository.GetGeneralConfiguration(ConfigurationName);
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

        public  async Task<ResponseDto> UpdateGeneralConfiguration(GeneralConfiguration request)
        {
            var response = new ResponseDto();
            try
            {
                var existsConfiguration = await this.generalConfigurationRepository.ExistsGeneralConfiguration(request.Name);
                if (!existsConfiguration)
                {
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("GeneralConfigurationNotFound", Messages.GeneralConfigurationNotFound);
                    return response;
                }

                response.Payload = await this.generalConfigurationRepository.UpdateGeneralConfiguration(request);
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
    }
}
