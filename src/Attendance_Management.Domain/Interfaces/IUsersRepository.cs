using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserDto> GetUser(string UserName);
        Task<AppUser> GetAppUser(string UserName);
        Task<UserDto> GetUserByEmployeCode(string empcode);
        Task<UserDto> GetUserById(string Id);
        Task<bool> ExistsUserByEmpCode(string UserName);
        Task<AppUser> GetAppUserByEmpCode(string empCode);
        Task<UsersPaginatedListDto> GetUsersPaginatedList(UsersPaginatedListDto request);
        Task<AreasUser> AddAreasUser(AreasUser request);
        Task<List<string>> GetUsersBySearch(string search);
        Task<AreasUser> GetAreaUserById(int Id);
        Task<AppUser> UpdateUser(string empCode, string firstName, string lastName, bool isActive);
        Task<AreasUser> UpdateAreaUser(AreasUser areaUse, int areaId, int positionId, bool isLeader);
        Task<bool> DeleteAreasUser(AreasUser request);
    }
}
