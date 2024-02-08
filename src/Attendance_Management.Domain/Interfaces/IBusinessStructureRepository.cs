using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface IBusinessStructureRepository
    {
        //deparment
        Task<List<DeparmentDto>> GetDeparments();
        Task<List<string>> GetDepartmentsName(string Search);
        Task<List<string>> GetDepartmentsCode(string Search);
        Task<DeparmentListPaginatedDto> GetDeparmentListPaginatedDto(DeparmentListPaginatedDto request);
        Task<Deparment> GetDeparment(int deparmentId);
        Task<DeparmentDto> GetDeparmentDto(int departmentId);
        Task<Deparment> GetDeparment(string deparmentId);
        Task<Deparment> AddDeparment(DeparmentDto deparmentDto);
        Task<Deparment> PutDeparment(DeparmentDto deparmentDto);
        Task<bool> DeparmentCodeExists(string code);
        Task<bool> DeleteDepartment(int id);


        //areas
        Task<List<AreaDto>> GetAreas();
        Task<List<string>> GetAreasName(string Search);
        Task<Area> GetArea(string areaCode);
        Task<Area> GetArea(int areaId);
        Task<AreaDto> GetAreaDto(int areaId);
        Task<Area> AddArea(AreaDto areaDto);
        Task<Area> PutArea(AreaDto areaDto);
        Task<bool> AreaCodeExists(string code);
        Task<AreaListPaginatedDto> GetAreaListPaginatedDto(AreaListPaginatedDto request);
        Task<bool> DeleteAreas(int id);

        //puestos
        Task<List<string>> GetPositionBySearch(string search);
        Task<PositionListPaginatedDto> GetPositionPaginatedListDto(PositionListPaginatedDto request);
        Task<List<PositionDto>> GetPositions();
        Task<Position> GetPosition(string positionCode);
        Task<Position> GetPosition(int positionId);
        Task<Position> AddPosition(PositionDto positionDto);
        Task<Position> PutPosition(PositionDto positionDto);
        Task<bool> PositionCodeExists(string code);
        Task<bool> DeletePosition(int id);

    }
}
