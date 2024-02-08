using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Attendance_Management.Services
{
    public class UsersServices : IUsersService
    {
        private readonly IUsersRepository usersRepository;
        private readonly IBusinessStructureService businessStructureService;
        private readonly IEmployeeService employeeService;
        private readonly ILogger<UsersServices> logger;

        public UsersServices(IUsersRepository usersRepository, ILogger<UsersServices> logger, IBusinessStructureService businessStructureService, IEmployeeService employeeService)
        {
            this.usersRepository = usersRepository;
            this.logger = logger;
            this.businessStructureService = businessStructureService;
            this.employeeService = employeeService;
        }

        public async Task<ResponseDto> AddAreasUser(AreasUser request)
        {
            var response = new ResponseDto();

            try
            {
                var areaExists = await this.businessStructureService.GetArea(request.AreaId);
                if (areaExists.Status != 200)
                {
                    return areaExists;
                }
                var userExists = await this.GetUserById(request.UserId);
                if (userExists.Status != 200)
                {
                    return userExists;
                }
                var positionExists = await this.businessStructureService.GetPositionById(request.PositionId);
                if (positionExists.Status != 200)
                {
                    return positionExists;
                }

                await this.usersRepository.AddAreasUser(request);
                response.Payload = request;
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

        public async Task<ResponseDto> DeleteAreasUser(int id)
        {
            var response = new ResponseDto();

            try
            {
                var areaUser = await this.usersRepository.GetAreaUserById(id);

                if (areaUser == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("AreaPuesto", Messages.AreasUserNotFound);
                    return response;

                }

                response.Payload = await this.usersRepository.DeleteAreasUser(areaUser);
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

        public async Task<ResponseDto> GetAreasUser(int id)
        {
            var response = new ResponseDto();

            try
            {
                var areasUserExists = await this.usersRepository.GetAreaUserById(id);

                if (areasUserExists == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("AreaPuesto", Messages.AreasUserNotFound);
                    return response;

                }

                response.Payload = areasUserExists;
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

        public async Task<ResponseDto> GetUser(string UserName)
        {
            var response = new ResponseDto();
            try
            {
                var user = await usersRepository.GetUser(UserName);
                if (user != null)
                {
                    user.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.FirstName.ToLower());
                    response.Payload = user;
                    response.Issuccessful = true;
                    response.Status = 200;
                }
                else
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("UserNotFound", Messages.UserNotFound);
                    return response;
                }
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

        public async Task<ResponseDto> GetUserbyEmpCode(string empCode)
        {
            var response = new ResponseDto();
            try
            {
                var user = await this.usersRepository.GetAppUserByEmpCode(empCode);
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(user.FirstName))
                        user.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.FirstName?.ToLower());
                    response.Payload = user;
                    response.Issuccessful = true;
                    response.Status = 200;
                }
                else
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("UserNotFound", Messages.UserNotFound);
                }
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

        public async Task<ResponseDto> GetUserById(string Id)
        {
            var response = new ResponseDto();
            try
            {
                var user = await this.usersRepository.GetUserById(Id);
                if (user != null)
                {
                    if(!string.IsNullOrEmpty(user.FirstName))
                    user.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.FirstName?.ToLower());
                    response.Payload = user;
                    response.Issuccessful = true;
                    response.Status = 200;
                }
                else
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("UserNotFound", Messages.UserNotFound);
                }
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

        public async Task<ResponseDto> GetUserDto(string empCode)
        {
            var response = new ResponseDto();
            try
            {
                var user = await this.usersRepository.ExistsUserByEmpCode(empCode);
                if (user)
                {
                    response.Payload = await this.usersRepository.GetUserByEmployeCode(empCode);
                    response.Issuccessful = true;
                    response.Status = 200;
                }
                else
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("UserNotFound", Messages.UserNotFound);
                }
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

        public async Task<ResponseDto> GetUsersBySearch(string request)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.usersRepository.GetUsersBySearch(request);
                response.Issuccessful = true;
                response.Status = 200;
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

        public async Task<ResponseDto> GetUsersPaginatedList(UsersPaginatedListDto request)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.usersRepository.GetUsersPaginatedList(request);
                response.Issuccessful = true;
                response.Status = 200;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Status = 500;
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }

            return response;
        }

        public async Task<ResponseDto> UpdateUser(UpdateUserDto request)
        {
            var response = new ResponseDto();

            try
            {
                var userResponseDto = await this.GetUserbyEmpCode(request.EmployeeCode);

                if (!userResponseDto.Issuccessful && userResponseDto.Status != 200)
                {
                    return userResponseDto;
                }
                AppUser userResponse = userResponseDto.Payload as AppUser;

                var empResponse = await this.employeeService.ExistsEmployee(request.EmployeeCode);

                if (!empResponse.Issuccessful || !(bool)empResponse.Payload)
                {
                    return empResponse;
                }

                var existsArea = await this.businessStructureService.GetArea(request.AreaCode);
                if (!existsArea.Issuccessful)
                {
                    return existsArea;
                }

                Area area = existsArea.Payload as Area;


                var existsPosition = await this.businessStructureService.GetPositionByCode(request.PositionCode);
                if (!existsPosition.Issuccessful)
                {
                    return existsPosition;
                }

                Position position = existsPosition.Payload as Position;

                await this.usersRepository.UpdateUser(request.EmployeeCode, request.FirstName, request.LastName, (bool)request.IsActive);

                if (request.AreasUserId > 0)
                {
                    var existsAreaUsers = await this.GetAreasUser(request.AreasUserId);
                    if (existsAreaUsers.Status != 200)
                    {
                        return existsAreaUsers;
                    }

                    AreasUser areasUser = existsAreaUsers.Payload as AreasUser;

                    await this.usersRepository.UpdateAreaUser(areasUser, area.Id, position.Id, request.IsLeaderArea);
                }
                else
                {
                    var areaUsers = new AreasUser()
                    {
                        AreaId = area.Id,
                        UserId = userResponse.Id,
                        IsLeader = request.IsLeaderArea,
                        PositionId = position.Id,
                    };

                    var areasUsers = await this.AddAreasUser(areaUsers);

                    if (areasUsers.Status != 200)
                    {
                        return areasUsers;
                    }
                }
                response = await this.GetUserDto(request.EmployeeCode);
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
