using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface IUsersService
    {
        Task<ResponseDto> GetUser(string UserName);
        Task<ResponseDto> GetUserbyEmpCode(string empCode);
        Task<ResponseDto> GetUserById(string Id);
        Task<ResponseDto> AddAreasUser(AreasUser request);
        Task<ResponseDto> GetUsersPaginatedList(UsersPaginatedListDto request);
        Task<ResponseDto> GetUsersBySearch(string request);
        Task<ResponseDto> GetUserDto(string empCode);
        Task<ResponseDto> GetAreasUser(int id);
        Task<ResponseDto> UpdateUser(UpdateUserDto request);
        Task<ResponseDto> DeleteAreasUser(int id);
        
    }
}
