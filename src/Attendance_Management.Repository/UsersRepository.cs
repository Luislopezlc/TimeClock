using Attendance_Management.Domain;
using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Extensions;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IProviderDbContext providerDbContext;
        private readonly AppDbContext appDbContext;
        public UsersRepository(IProviderDbContext providerDbContext)
        {
            this.providerDbContext = providerDbContext;
            this.appDbContext = this.providerDbContext.GetAppDbContext();
        }

        public async Task<AreasUser> AddAreasUser(AreasUser request)
        {
           await this.appDbContext.AddAsync(request);   
           await this.appDbContext.SaveChangesAsync();
           return request;
        }

        public  async Task<bool> DeleteAreasUser(AreasUser areasUser)
        {
            this.appDbContext.areasUsers.Remove(areasUser);
            await this.appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsUserByEmpCode(string empCode)
        {
            var response = await this.appDbContext.AppUsers.Where(u => u.EmployeeCode.Equals(empCode)).FirstOrDefaultAsync();
            return response != null;
        }

        public async Task<AppUser> GetAppUser(string UserName)
        {
            var appUser = await this.appDbContext.AppUsers.Where(u => u.UserName.Equals(UserName)).FirstOrDefaultAsync();
            return appUser;
        }

        public async Task<AppUser> GetAppUserByEmpCode(string empCode)
        {
            var response = await this.appDbContext.AppUsers.Where(u => u.EmployeeCode.Equals(empCode)).FirstOrDefaultAsync();
            return response;
        }

        public async Task<AreasUser> GetAreaUserById(int Id)
        {
           var response = await this.appDbContext.areasUsers.Where(x => x.Id == Id).FirstOrDefaultAsync();
           return response;
        }

        public async Task<UserDto> GetUser(string username)
        {

            var response = await (from u in this.appDbContext.AppUsers
                                  join ur in this.appDbContext.UserRoles on u.Id equals ur.UserId into usersRoles
                                  from ur in usersRoles.DefaultIfEmpty()
                                  join r in this.appDbContext.Roles on ur.RoleId equals r.Id into roles
                                  from r in roles.DefaultIfEmpty()
                                  where u.UserName.ToLower().Equals(username.ToLower())
                                  select new UserDto
                                  {
                                      Id = u.EmployeeCode,
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      Email = u.Email,
                                      Rol = r != null ? r.Name : "Sin rol",
                                      IsActive = (bool)u.IsActive,
                                      AreasUsers = (from a in this.appDbContext.areas
                                                    join au in this.appDbContext.areasUsers on a.Id equals au.AreaId
                                                    join po in this.appDbContext.positions on au.PositionId equals po.Id
                                                    join dep in this.appDbContext.deparments on a.DeparmentId equals dep.Id
                                                    where au.UserId == u.Id
                                                    select new AreasUsersDto
                                                    {
                                                        Id = au.Id,
                                                        Department = dep.Name,
                                                        Area = a.Name,
                                                        Position = po.Name,
                                                        IsLeader = au.IsLeader
                                                    }
                                                          ).ToList()
                                  }
                                  ).FirstOrDefaultAsync();
            return response;
        }


        public async Task<UserDto> GetUserByEmployeCode(string empcode)
        {
            var response = await (from u in this.appDbContext.AppUsers
                                  join ur in this.appDbContext.UserRoles on u.Id equals ur.UserId into usersRoles
                                  from ur in usersRoles.DefaultIfEmpty()
                                  join r in this.appDbContext.Roles on ur.RoleId equals r.Id into roles
                                  from r in roles.DefaultIfEmpty()
                                  where u.EmployeeCode.Equals(empcode)
                                  select new UserDto
                                  {
                                      Id = u.EmployeeCode,
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      Email = u.Email,
                                      Rol = r != null ? r.Name : "Sin rol",
                                      IsActive = (bool) u.IsActive,
                                      AreasUsers =  (from a in this.appDbContext.areas
                                                          join au in this.appDbContext.areasUsers on a.Id equals au.AreaId
                                                          join po in this.appDbContext.positions on au.PositionId equals po.Id
                                                          join dep in this.appDbContext.deparments on a.DeparmentId equals dep.Id
                                                          where au.UserId == u.Id
                                                          select new AreasUsersDto
                                                          {
                                                              Id = au.Id,
                                                              Department = dep.DeparmentCode,
                                                              Area = a.AreaCode,
                                                              Position = po.PositionCode,
                                                              IsLeader = au.IsLeader
                                                          }
                                                          ).ToList()
                                  }
                                  ).FirstOrDefaultAsync();

            return response;
        }

        public async Task<UserDto> GetUserById(string Id)
        {
            var response = new UserDto();

            response = await (from u in this.appDbContext.AppUsers
                              join ur in this.appDbContext.UserRoles on u.Id equals ur.UserId into usersRoles
                              from ur in usersRoles.DefaultIfEmpty()
                              join r in this.appDbContext.Roles on ur.RoleId equals r.Id into roles
                              from r in roles.DefaultIfEmpty()
                              where u.Id.Equals(Id)
                              select new UserDto
                              {
                                  Id = u.EmployeeCode,
                                  FirstName = u.FirstName,
                                  Email= u.Email,
                                  LastName = u.LastName,
                                  Rol = r != null ? r.Name : "Sin rol",
                                  IsActive = (bool)u.IsActive,
                                  AreasUsers = (from a in this.appDbContext.areas
                                                join au in this.appDbContext.areasUsers on a.Id equals au.AreaId
                                                join po in this.appDbContext.positions on au.PositionId equals po.Id
                                                join dep in this.appDbContext.deparments on a.DeparmentId equals dep.Id
                                                where au.UserId == u.Id
                                                select new AreasUsersDto
                                                {
                                                    Id = au.Id,
                                                    Department = dep.Name,
                                                    Area = a.Name,
                                                    Position = po.Name,
                                                    IsLeader = au.IsLeader
                                                }
                                                      ).ToList()
                              }
                                  ).FirstOrDefaultAsync();

            return response;
        }

        public async Task<List<string>> GetUsersBySearch(string search)
        {
            var response = new List<string>();
            if (!string.IsNullOrEmpty(search))
            {
                var searchToUpper = search.ToUpper();
                response = await (from u in this.appDbContext.AppUsers
                                  where u.FirstName.ToUpper().StartsWith(searchToUpper) ||
                                  u.LastName.ToUpper().StartsWith(searchToUpper) ||
                                  u.EmployeeCode.Contains(searchToUpper)
                                  select $"{u.EmployeeCode} - {u.FirstName} {u.LastName}" 
                                  ).Take(10).ToListAsync();
            }
            else 
            {
                response = await (from u in this.appDbContext.AppUsers
                                  select $"{u.EmployeeCode} - {u.FirstName} {u.LastName}"
                                 ).Take(10).ToListAsync();
            }

            return response;
        }

        public async Task<UsersPaginatedListDto> GetUsersPaginatedList(UsersPaginatedListDto request)
        {
            var response = new UsersPaginatedListDto();
            response.Pagination = request.Pagination;
            if (string.IsNullOrEmpty(request.Search))
            {
                response.Users = await (from u in this.appDbContext.AppUsers
                                        join ur in this.appDbContext.UserRoles on u.Id equals ur.UserId into usersRoles
                                        from ur in usersRoles.DefaultIfEmpty()
                                        join r in this.appDbContext.Roles on ur.RoleId equals r.Id into roles
                                        from r in roles.DefaultIfEmpty()
                                        select new UserDto
                                        {
                                            Id = u.EmployeeCode,
                                            FirstName = u.FirstName,
                                            Email = u.Email,
                                            LastName = u.LastName,
                                            Rol = r != null ? r.Name : "Sin rol",
                                            IsActive = u.IsActive != null ? (bool)u.IsActive : false,
                                        }
                                        ).OrderBy(u => u.FirstName)
                                        .Pagination(request.Pagination)
                                        .ToListAsync();

                response.Pagination.TotalRecords = this.appDbContext.AppUsers.Count();
            }
            else
            {
                var search = request.Search.ToUpper();
                response.Users = await (from u in this.appDbContext.AppUsers
                                        join ur in this.appDbContext.UserRoles on u.Id equals ur.UserId into usersRoles
                                        from ur in usersRoles.DefaultIfEmpty()
                                        join r in this.appDbContext.Roles on ur.RoleId equals r.Id into roles
                                        from r in roles.DefaultIfEmpty()
                                        where u.FirstName.ToUpper().StartsWith(search) ||
                                        u.LastName.ToUpper().StartsWith(search) ||
                                        u.EmployeeCode.Contains(search)
                                        select new UserDto
                                        {
                                            Id = u.EmployeeCode,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            Email = u.Email,
                                            Rol = r != null ? r.Name : "Sin rol",
                                            IsActive = (bool)u.IsActive
                                        }
                                        ).OrderBy(u => u.FirstName)
                                        .Pagination(request.Pagination)
                                        .ToListAsync();

                response.Pagination.TotalRecords = response.Users.Count();
            }

            double dividend = Convert.ToDouble(response.Pagination.TotalRecords);
            double divisor = Convert.ToDouble(response.Pagination.RecordsForPage);

            //10.3
            double result = dividend / divisor;
            //10
            var resultWithoutDecimals = Math.Floor(result);

            if (resultWithoutDecimals != result)
            {
                response.Pagination.TotalPages = Convert.ToInt32(resultWithoutDecimals) + 1;
            }
            else
            {
                response.Pagination.TotalPages = Convert.ToInt32(result);
            }

            return response;

        }

        public async Task<AreasUser> UpdateAreaUser(AreasUser areaUser, int areaId, int positionId, bool isLeader)
        {
            areaUser.AreaId = areaId;
            areaUser.PositionId = positionId;
            areaUser.IsLeader = isLeader;


            this.appDbContext.Attach(areaUser);
            this.appDbContext.Entry(areaUser).State = EntityState.Modified;
            await this.appDbContext.SaveChangesAsync();

            return areaUser;
        }

        public async Task<AppUser> UpdateUser(string empCode, string firstName, string lastName, bool isActive)
        {
            var updateUser = await this.appDbContext.AppUsers.Where(x => x.EmployeeCode.Equals(empCode)).FirstOrDefaultAsync();

            updateUser.FirstName= firstName;
            updateUser.LastName= lastName;  
            updateUser.IsActive = isActive;

            this.appDbContext.Attach(updateUser);
            this.appDbContext.Entry(updateUser).State = EntityState.Modified;
            await this.appDbContext.SaveChangesAsync();

            return updateUser;
        }
    }
}
