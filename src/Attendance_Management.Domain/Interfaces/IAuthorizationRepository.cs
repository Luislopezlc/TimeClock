using Attendance_Management.Domain.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<List<SidebarDto>> GetSidebar(string userName);
        Task<List<RolDto>> GetRolDto();
        Task<IdentityRole> GetRolDtoById(string Id);
        Task<List<RolDto>> GetRolesBySearch(string request);
        Task<IdentityRole> AddRolesDto(RolDto request, IdentityRole rolExist);
        Task<bool> DeletRol(IdentityRole request);
        Task<string> GetRolIdByUserName(string userName);

        Task<bool> ExistsView(string viewName);
        Task<bool> CanYouSeeTheView(string viewName, string username);
    }
}
