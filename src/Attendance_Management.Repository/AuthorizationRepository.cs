using Attendance_Management.Domain;
using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Repository
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly IProviderDbContext providerDbContext;
        private readonly IUsersRepository usersRepository;
        private readonly AppDbContext appDbContext;
        public AuthorizationRepository(IProviderDbContext providerDbContext, IUsersRepository usersRepository)
        {
            this.providerDbContext = providerDbContext;
            this.usersRepository = usersRepository;
            this.appDbContext = this.providerDbContext.GetAppDbContext();
        }

        public async Task<List<SidebarDto>> GetSidebar(string userName)
        {
            var user = await this.usersRepository.GetAppUser(userName);

            var rol = await this.appDbContext.UserRoles.Where(r => r.UserId.Equals(user.Id)).FirstOrDefaultAsync();

            var response = await ( from au in this.appDbContext.auth_sidebar 
                                   join sr in this.appDbContext.sidebarRoles on au.Id equals sr.AuthId
                                   where au.IsActive && au.IsTitle && sr.RolId.Equals(rol.RoleId)
                                   select au).ToListAsync();

            var sidebarWithSecundaries = new List<SidebarDto>();


            foreach(var title in response)
            {
                var secundaries = await (from side in this.appDbContext.auth_sidebar
                                         join sr in this.appDbContext.sidebarRoles on side.Id equals sr.AuthId
                                         where side.IsActive && !side.IsTitle && sr.RolId.Equals(rol.RoleId) &&
                                         side.Code.Equals(title.Code)
                                         orderby side.Priority
                                         select new SidebarSecondaryDto
                                         {
                                             Id= side.Id,
                                             Name = side.Name,
                                             IsActive = side.IsActive,
                                             IsTitle= side.IsTitle,
                                             Code= side.Code,
                                             Controller = side.Controller,
                                             Action = side.Action,
                                         }).ToListAsync();
                                         

                var sidebarTitle = new SidebarDto()
                {
                    Id= title.Id,
                    Name= title.Name,
                    Code= title.Code,
                    IsActive= title.IsActive,
                    IsTitle = title.IsTitle
                };

                sidebarTitle.SidebarSecondaries = secundaries;

                sidebarWithSecundaries.Add(sidebarTitle);
            }

            sidebarWithSecundaries.OrderBy(s => s.Priority).ToList();

            return sidebarWithSecundaries;
        }

        public async Task<List<RolDto>> GetRolDto ()
        {
            var response = await (from rol in this.appDbContext.Roles
                                  select new RolDto
                                  {
                                      Name = rol.Name,
                                      NormalizedName = rol.NormalizedName,
                                      CurrencyStamp = rol.ConcurrencyStamp,
                                  }).ToListAsync();
            return response;
        }

        public async Task<IdentityRole> GetRolDtoById(string id)
        {
            var response = await this.appDbContext.Roles
                                        .Where(r => r.Id == id)
                                        .FirstOrDefaultAsync();

            return response;
        }

        public async Task<List<RolDto>> GetRolesBySearch(string request)
        {
            var response = await (from rol in this.appDbContext.Roles
                              where rol.NormalizedName.Trim().StartsWith(request.ToUpper().Trim())
                              select new RolDto
                              {
                                  Name = rol.Name,
                                  NormalizedName = rol.NormalizedName,
                                  CurrencyStamp = rol.ConcurrencyStamp,
                              }).ToListAsync();

            return response;
        }

        public async Task<IdentityRole> AddRolesDto(RolDto request, IdentityRole rolExist)
        {
            var response = new IdentityRole();
            if(rolExist != null)
            {
                rolExist.Name = request.Name;
                rolExist.NormalizedName = request.NormalizedName;
                rolExist.ConcurrencyStamp = request.CurrencyStamp;
            }
            else
            {
                var rol = new IdentityRole()
                {
                    Name = request.Name,
                    NormalizedName = request.NormalizedName,
                    ConcurrencyStamp = request.CurrencyStamp,
                };

                await this.appDbContext.AddAsync(rol);
            }

            await this.appDbContext.SaveChangesAsync();
            return response;
        }

        public async Task<bool> DeletRol(IdentityRole request)
        {
            if(request != null)
            {
                this.appDbContext.Remove(request);
                await this.appDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> GetRolIdByUserName(string userName)
        {
            var appUser = await this.appDbContext.AppUsers.Where(u => u.UserName.Equals(userName)).FirstOrDefaultAsync();
            var response = await this.appDbContext.UserRoles.Where(r => r.UserId.Equals(appUser.Id)).Select(x => x.RoleId).FirstOrDefaultAsync();

            return response;
        }

        public async Task<bool> CanYouSeeTheView(string viewName, string username)
        {
            var sidebar = await this.GetSidebar(username);
            var existViewInSidebar = sidebar.Where(e => 
                        e.SidebarSecondaries.Where(x => 
                            x.Action.Equals(viewName,StringComparison.OrdinalIgnoreCase)).FirstOrDefault() != null).FirstOrDefault();

            return existViewInSidebar != null;
        }

        public async Task<bool> ExistsView(string viewName)
        {
           var existsView = await this.appDbContext.auth_sidebar.Where(x=> x.Action.Equals(viewName)).FirstOrDefaultAsync();
           return existsView != null;
        }
    }
}
