using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Attendance_Management.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository authorizationRepository;
        private readonly ILogger<AuthorizationService> logger;

        public AuthorizationService(IAuthorizationRepository authorizationRepository, ILogger<AuthorizationService> logger)
        {
            this.authorizationRepository = authorizationRepository;
            this.logger = logger;
        }

        public async Task<ResponseDto> GetSidebar(string userName)
        {
            var response = new ResponseDto();
            try
            {

                var thereIsRol = await authorizationRepository.GetRolIdByUserName(userName);
                if (string.IsNullOrEmpty(thereIsRol)) 
                {
                    response.Issuccessful = false;
                    response.Errors = ErrorMessages.AddMessageError("Rol", Messages.UserRolNotFound);
                    response.Status = 400;
                    return response;
                }

                response.Payload = await this.authorizationRepository.GetSidebar(userName);
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

        public async Task<ResponseDto> GetRolDto()
        {
            var response = new ResponseDto();
            try
            {
                var rolDto = await this.authorizationRepository.GetRolDto();
                response.Payload = JsonSerializer.Serialize(rolDto);
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

        public async Task<ResponseDto> GetRolesBySearch(string request)
        {
            var response = new ResponseDto();
            try
            {
                var roles = await this.authorizationRepository.GetRolesBySearch(request);
                response.Payload = JsonSerializer.Serialize(roles);
                response.Issuccessful = true;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = true;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<ResponseDto> AddRolDto(RolDto request)
        {
            var response = new ResponseDto();

            try
            {
                var rolExist = await this.authorizationRepository.GetRolDtoById(request.Id);

                response.Payload = await this.authorizationRepository.AddRolesDto(request, rolExist);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = true;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<ResponseDto> DeleteRol(string Id)
        {
            var response = new ResponseDto();

            try
            {
                var rolDelete = await this.authorizationRepository.GetRolDtoById(Id);
                var isDelete = await this.authorizationRepository.DeletRol(rolDelete);

                if (isDelete)
                {
                    response.Payload = isDelete;
                    response.Issuccessful = true;
                }
                else
                {
                    response.Payload = isDelete;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("RolNotDelete", Messages.RolNotDelete);
                    return response;
                }
            } 
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = true;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<ResponseDto> CanYouSeeTheView(string viewName, string username)
        {
            var response = new ResponseDto();

            try
            {
                var existsView = await this.authorizationRepository.ExistsView(viewName);
                if (existsView)
                {
                    var canYouSeeTheView = await this.authorizationRepository.CanYouSeeTheView(viewName, username);
                    if (canYouSeeTheView)
                    {
                        response.Status = 200;
                        response.Issuccessful = true;
                    }
                    else 
                    {
                        response.Issuccessful = false;
                        response.Status = 403;
                    }
                    response.Payload = canYouSeeTheView;
                }
                else 
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("NoExistsView", Messages.NoExistsView);
                    return response;
                }

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = true;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }
    }
}
