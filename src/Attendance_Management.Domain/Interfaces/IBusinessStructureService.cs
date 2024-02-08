using Attendance_Management.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface IBusinessStructureService
    {
        //departamentos
        Task<ResponseDto> GetDeparments();
        Task<ResponseDto> GetDeparmentsName(string Search);
        Task<ResponseDto> GetDeparmentsDto(int deparmentId);
        Task<ResponseDto> GetDeparmentDto(string code);
        Task<ResponseDto> DeleteDepartment(int DepartmentId);
        Task<ResponseDto> PostDeparments(DeparmentDto deparmentDto);
        Task<ResponseDto> GetDeparmentsListPaginatedDto(DeparmentListPaginatedDto request);
        Task<ResponseDto> GetDepartmentsCode(string Search);

        //areas
        Task<ResponseDto> GetAreas();
        Task<ResponseDto> GetAreasName(string Search);
        Task<ResponseDto> GetArea(int areaId);
        Task<ResponseDto> GetArea(string code);

        Task<ResponseDto> GetAreaDto(int areaId);
        Task<ResponseDto> PostArea(AreaDto areaDto);
        Task<ResponseDto> GetAreaListPaginatedDto(AreaListPaginatedDto request);
        Task<ResponseDto> DeleteArea(int areaId);

        //puestos
        Task<ResponseDto> GetPositions();
        Task<ResponseDto> PostPosition(PositionDto positionDto);
        Task<ResponseDto> GetPositionListPaginatedDto(PositionListPaginatedDto request);
        Task<ResponseDto> GetPositionById(int id);
        Task<ResponseDto> GetPositionByCode(string code);
        Task<ResponseDto> GetPositionsBySearch(string request);
        Task<ResponseDto> DeletePosition(int positionId);
    }
}
