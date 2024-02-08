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
    public class BusinessStructureService : IBusinessStructureService
    {
        private readonly IBusinessStructureRepository businessStructureRepository;
        private readonly ILogger<BusinessStructureService> logger;
        private readonly IUsersRepository usersRepository;
        public BusinessStructureService(IBusinessStructureRepository businessStructureRepository, 
            ILogger<BusinessStructureService> logger, IUsersRepository usersRepository)
        {
            this.businessStructureRepository = businessStructureRepository;
            this.logger = logger;
            this.usersRepository = usersRepository;
        }

        public async Task<ResponseDto> GetAreaListPaginatedDto(AreaListPaginatedDto request)
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.businessStructureRepository.GetAreaListPaginatedDto(request);
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

        public async Task<ResponseDto> GetAreas()
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.businessStructureRepository.GetAreas();
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

        public async Task<ResponseDto> GetAreasName(string Search)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.businessStructureRepository.GetAreasName(Search);
                response.Issuccessful = true;
                response.Status = 200;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful= false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
            }

            return response;
        }

        public async Task<ResponseDto> GetArea(int areaId)
        {
            var response = new ResponseDto();

            try
            {
                var area = await this.businessStructureRepository.GetArea(areaId);
                if (area == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Area", Messages.AreaNotFound);
                    return response;
                }
                response.Payload = area;
                response.Issuccessful = true;
                response.Status = 200;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful= false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<ResponseDto> GetDeparments()
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.businessStructureRepository.GetDeparments();
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

        public async Task<ResponseDto> GetDeparmentsName(string Search)
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.businessStructureRepository.GetDepartmentsName(Search);
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

        public async Task<ResponseDto> GetDeparmentsListPaginatedDto(DeparmentListPaginatedDto request)
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.businessStructureRepository.GetDeparmentListPaginatedDto(request);
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

        public async Task<ResponseDto> GetPositionById(int id)
        {
            var response = new ResponseDto();

            try
            {
                var position = await this.businessStructureRepository.GetPosition(id);
                if (position == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Puesto", Messages.PositionNotFound);
                    return response;
                }
                response.Payload = position;
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

        public async Task<ResponseDto> GetPositionListPaginatedDto(PositionListPaginatedDto request)
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.businessStructureRepository.GetPositionPaginatedListDto(request);
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

        public async Task<ResponseDto> GetPositions()
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.businessStructureRepository.GetPositions();
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

        public async Task<ResponseDto> GetPositionsBySearch(string request)
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.businessStructureRepository.GetPositionBySearch(request);
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

        public async Task<ResponseDto> PostArea(AreaDto areaDto)
        {
            var response = new ResponseDto();

            try
            {
                var deptoExist = await this.businessStructureRepository?.GetDeparment(areaDto.DepartmentName);

                if (deptoExist == null)
                {
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Departamento", Messages.DepartmentNotFound);
                    return response;

                }
                areaDto.DeparmentId = deptoExist.Id;
                if (!string.IsNullOrEmpty(areaDto.EmployeeCode))
                {
                    var existsUser = await this.usersRepository.GetUserByEmployeCode(areaDto.EmployeeCode);
                    if (existsUser == null)
                    {
                        response.Issuccessful = false;
                        response.Status = 400;
                        response.Errors = ErrorMessages.AddMessageError("Jefe", Messages.EmployeeNotFound);
                        return response;

                    }
                }
                var area = await this.businessStructureRepository.GetArea(areaDto.Id);

                if (area != null)
                {
                    var codeExists = await this.businessStructureRepository.AreaCodeExists(areaDto.Code);

                    if (codeExists && area.Id != areaDto.Id)
                    {
                        response.Issuccessful = false;
                        response.Status = 400;
                        response.Errors = ErrorMessages.AddMessageError("Codigo", Messages.AreaCodeDuplicate);
                        return response;

                    }

                    //actualizamos
                    var areaUpdate = await this.businessStructureRepository.PutArea(areaDto);
                    response.Payload = areaUpdate;
                }
                else
                {
                    var codeExists = await this.businessStructureRepository.AreaCodeExists(areaDto.Code);

                    if (codeExists)
                    {
                        response.Issuccessful = false;
                        response.Status = 400;
                        response.Errors = ErrorMessages.AddMessageError("Codigo", Messages.AreaCodeDuplicate);
                        return response;

                    }
                    //insertamos
                    var areaInsert = await this.businessStructureRepository.AddArea(areaDto);
                    response.Payload = areaInsert;
                }
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

        public async Task<ResponseDto> PostDeparments(DeparmentDto deparment)
        {
            var response = new ResponseDto();   
            try
            {
                
                var existsUser = await this.usersRepository.GetUserByEmployeCode(deparment.EmployeeCode);
                if(existsUser == null )
                {
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Jefe", Messages.EmployeeNotFound);
                    return response;

                }
                var depto = await this.businessStructureRepository.GetDeparment(deparment.Id);
                if (depto != null)
                {
                    //actualizar
                    var codeExists = await this.businessStructureRepository.DeparmentCodeExists(deparment.Code);
                    if (codeExists && depto.Id != deparment.Id)
                    {
                        response.Issuccessful = false;
                        response.Status = 400;
                        response.Errors = ErrorMessages.AddMessageError("Codigo", Messages.DepartmentCodeDuplicated);
                        return response;

                    }
                    response.Payload = await this.businessStructureRepository.PutDeparment(deparment);
                }
                else 
                {
                    response.Payload = await this.businessStructureRepository.AddDeparment(deparment);
                }
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

        public async Task<ResponseDto> PostPosition(PositionDto positionDto)
        {
            var response = new ResponseDto();
            try
            {
                var codeExists = await this.businessStructureRepository.PositionCodeExists(positionDto.Code);
                if (codeExists)
                {
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Codigo", Messages.PositionCodeDuplicated);
                }

                var position = await this.businessStructureRepository.GetPosition(positionDto.Id);
                if (position != null)
                {
                    //actualizamos
                    response.Payload = await this.businessStructureRepository.PutPosition(positionDto);
                }
                else
                {
                    response.Payload = await this.businessStructureRepository.AddPosition(positionDto);
                }
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

        public async Task<ResponseDto> GetDeparmentsDto(int deparmentId)
        {
            var response = new ResponseDto();

            try
            {
                var department = await this.businessStructureRepository.GetDeparmentDto(deparmentId);
                if (department == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Departamento", Messages.DepartmentNotFound);
                    return response;

                }
                response.Payload = department;
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

        public async Task<ResponseDto> GetAreaDto(int areaId)
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.businessStructureRepository.GetAreaDto(areaId);
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

        public async Task<ResponseDto> GetDepartmentsCode(string Search)
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.businessStructureRepository.GetDepartmentsCode(Search);
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

        public async Task<ResponseDto> GetArea(string code)
        {
            var response = new ResponseDto();

            try
            {
                var area = await this.businessStructureRepository.GetArea(code);
                if (area == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Area", Messages.AreaNotFound);
                    return response;

                }
                response.Payload = area;
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

        public async Task<ResponseDto> GetDeparmentDto(string code)
        {
            var response = new ResponseDto();

            try
            {
                var department = await this.businessStructureRepository.GetDeparment(code);
                if (department == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Departamento", Messages.DepartmentNotFound);
                    return response;

                }
                response.Payload = department;
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

        public async Task<ResponseDto> GetPositionByCode(string code)
        {
            var response = new ResponseDto();

            try
            {
                var position = await this.businessStructureRepository.GetPosition(code);
                if (position == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Puesto", Messages.PositionNotFound);
                    return response;

                }
                response.Payload = position;
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

        public async Task<ResponseDto> DeletePosition(int positionId)
        {
            var response = new ResponseDto();

            try
            {
                var position = await this.businessStructureRepository.GetPosition(positionId);
                if (position == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Puesto", Messages.PositionNotFound);
                    return response;
                }
                else 
                {
                   var deleted =  await this.businessStructureRepository.DeletePosition(positionId);
                   response.Payload = deleted;
                }

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

        public async Task<ResponseDto> DeleteArea(int areaId)
        {
            var response = new ResponseDto();

            try
            {
                var area = await this.businessStructureRepository.GetArea(areaId);
                if (area == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Area", Messages.AreaNotFound);
                    return response;
                }
                else
                {
                    var deleted = await this.businessStructureRepository.DeleteAreas(areaId);
                    response.Payload = deleted;
                }

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

        public async Task<ResponseDto> DeleteDepartment(int DepartmentId)
        {
            var response = new ResponseDto();

            try
            {
                var department = await this.businessStructureRepository.GetDeparment(DepartmentId);
                if (department == null)
                {
                    response.Payload = null;
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("Departamento", Messages.DepartmentNotFound);
                    return response;
                }
                else
                {
                    var deleted = await this.businessStructureRepository.DeleteDepartment(DepartmentId);
                    response.Payload = deleted;
                }

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
