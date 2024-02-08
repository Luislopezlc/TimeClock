using Attendance_Management.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface IAuthorizationService
    {
        Task<ResponseDto> GetSidebar(string userName);
        Task<ResponseDto> GetRolDto();
        Task<ResponseDto> GetRolesBySearch(string request);
        Task<ResponseDto> AddRolDto(RolDto request);
        Task<ResponseDto> DeleteRol(string Id);
        Task<ResponseDto> CanYouSeeTheView(string viewName, string username);

    }
}
