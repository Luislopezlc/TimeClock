using Attendance_Management.Domain;
using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Extensions;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Repository
{
    public class BusinessStructureRepository : IBusinessStructureRepository
    {
        private readonly IProviderDbContext providerDbContext;
        private readonly AppDbContext AppDbContext;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IUsersRepository usersRepository;
        public BusinessStructureRepository(IProviderDbContext providerDbContex, IEmployeeRepository employeeRepository, IUsersRepository usersRepository)

        {
            this.providerDbContext = providerDbContex;
            this.AppDbContext =  this.providerDbContext.GetAppDbContext();
            this.employeeRepository = employeeRepository;
            this.usersRepository = usersRepository;
        }


        public async Task<Area> GetArea(string areaCode)
        {
            var response = await this.AppDbContext.areas.Where(x => x.AreaCode.Equals(areaCode)).FirstOrDefaultAsync();
            return response;
        }

        public async Task<List<string>> GetAreasName(string Search)
        {
            var response = new List<string>();
            if (!string.IsNullOrEmpty(Search))
            {
                var searchUpper = Search.ToUpper();

                response = await (from area in this.AppDbContext.areas
                                  where area.Name.ToUpper().StartsWith(searchUpper) ||
                                  area.Name.ToUpper().Contains(searchUpper)
                                  select area.Name
                                  ).ToListAsync();
            }
            else 
            {
                response = await (from area in this.AppDbContext.areas
                                  select area.Name).ToListAsync();
            }
            return response;
        }
        public async Task<Area> GetArea(int areaId)
        {
            var response = await this.AppDbContext.areas.FindAsync(areaId);
            return response;
        }

        public async Task<AreaDto> GetAreaDto(int areaId)
        {
            var response = await (
                                    from area in this.AppDbContext.areas
                                    join depts in this.AppDbContext.deparments on area.DeparmentId equals depts.Id
                                    join areaUsers in this.AppDbContext.areasUsers on area.Id equals areaUsers.AreaId into au
                                    from leftua in au.DefaultIfEmpty()
                                    join user in this.AppDbContext.Users on leftua.UserId equals user.Id into usera
                                    from ausu in usera.DefaultIfEmpty()
                                    where area.Id == areaId 
                                    select new AreaDto
                                    {
                                        Id = area.Id,
                                        Name = area.Name,
                                        Code = area.AreaCode,
                                        DepartmentName = depts.Name,
                                        DepartmentCode = depts.DeparmentCode,
                                        EmployeeCode = ausu.EmployeeCode,
                                        FullName = (ausu.EmployeeCode != null) ? $"{ausu.EmployeeCode} - {ausu.FirstName} {ausu.LastName}" : "Sin director de area"
                                    }
                                ).FirstOrDefaultAsync();

            //var area = await this.AppDbContext.areas.Where(a => a.Id == areaId).FirstOrDefaultAsync();



            return response;
        }


        public async Task<Area> AddArea(AreaDto areaDto)
        {
            var area = new Area()
            {
                Name = areaDto.Name,
                AreaCode = areaDto.Code,
                DeparmentId = areaDto.DeparmentId
            };

            await this.AppDbContext.AddAsync(area);
            await this.AppDbContext.SaveChangesAsync();

            return area;
        }

        public async Task<Area> PutArea(AreaDto areaDto)
        {
            var area = await  this.AppDbContext.areas.FindAsync(areaDto.Id);
            area.Name = areaDto.Name;
            area.DeparmentId = areaDto.DeparmentId;
            area.AreaCode= areaDto.Code;

            this.AppDbContext.Attach(area);
            this.AppDbContext.Entry(area).State = EntityState.Modified;

            await this.AppDbContext.SaveChangesAsync();
            return area;
        }

        public async Task<bool> AreaCodeExists(string code)
        {
            var response = await this.AppDbContext.areas.Where(x => x.AreaCode.Equals(code)).FirstOrDefaultAsync();
            return response != null;
        }

        public async Task<List<AreaDto>> GetAreas()
        {
           var response  = await (from area in this.AppDbContext.areas
                                  join depts in this.AppDbContext.deparments on area.DeparmentId equals depts.Id
                                  join areaUsers in this.AppDbContext.areasUsers on area.Id equals areaUsers.AreaId
                                  join user in this.AppDbContext.Users on areaUsers.UserId equals user.Id
                                  select new AreaDto
                                  {
                                      Id = area.Id,
                                      Name = area.Name,
                                      Code = area.AreaCode,
                                      DepartmentName = depts.Name,
                                      FullName = this.employeeRepository.GetEmployeeDto(user.EmployeeCode).Result.FullName
                                  }
                                  ).ToListAsync();
            return response;
        }
        public async Task<Deparment> AddDeparment(DeparmentDto deparment)
        {
            var appUser = await this.usersRepository.GetAppUserByEmpCode(deparment.EmployeeCode);
            
            var entityInsert = new Deparment()
            {
                DeparmentCode = deparment.Code,
                UserId = appUser.Id,
                Name = deparment.Name
            };

            await this.AppDbContext.AddAsync(entityInsert);
            await this.AppDbContext.SaveChangesAsync();

            return entityInsert;
        }

        public async Task<List<DeparmentDto>> GetDeparments()
        {
            var response = await ( from departs in this.AppDbContext.deparments
                                   select new DeparmentDto
                                   {
                                       Id = departs.Id,
                                       Name = departs.Name,
                                       Code = departs.DeparmentCode,
                                       FullName = this.employeeRepository.GetEmployeeDto
                                       (departs.User.EmployeeCode).Result.FullName
                                   }
                                   ).ToListAsync();
            return response;
        }

        public async Task<List<string>> GetDepartmentsName(string Search)
        {
            var response = new List<string>();
            if (!string.IsNullOrEmpty(Search))
            {
                var searchUpper = Search.ToUpper();

                response = await (from dep in this.AppDbContext.deparments
                                  where dep.Name.ToUpper().StartsWith(searchUpper) ||
                                  dep.Name.ToUpper().Contains(searchUpper)
                                  select dep.Name
                                  ).Take(10).ToListAsync();
            }
            else 
            {
                response = await (from dep in this.AppDbContext.deparments
                                  select dep.Name
                                  ).Take(10).ToListAsync();
            }

            return response;
        }

        public async Task<List<string>> GetDepartmentsCode(string Search)
        {
            var response = new List<string>();
            if (!string.IsNullOrEmpty(Search))
            {
                var searchUpper = Search.ToUpper();

                response = await (from dep in this.AppDbContext.deparments
                                  where dep.Name.ToUpper().StartsWith(searchUpper) ||
                                  dep.Name.ToUpper().Contains(searchUpper)
                                  select dep.DeparmentCode
                                  ).Take(10).ToListAsync();
            }
            else
            {
                response = await (from dep in this.AppDbContext.deparments
                                  select dep.DeparmentCode
                                  ).Take(10).ToListAsync();
            }

            return response;
        }

        public async Task<Deparment> PutDeparment(DeparmentDto deparment)
        {
            var appUser = await this.usersRepository.GetUserByEmployeCode(deparment.EmployeeCode);

            var exists = await this.AppDbContext.deparments.FindAsync(deparment.Id);

            exists.DeparmentCode = deparment.Code;
            exists.Name = deparment.Name;
            exists.UserId = appUser.Id;

            this.AppDbContext.Attach(exists);
            this.AppDbContext.Entry(exists).State = EntityState.Modified;
            await this.AppDbContext.SaveChangesAsync();
            return exists;
            
        }

        public async Task<Deparment>GetDeparment(string code)
        {
            var deparment = await this.AppDbContext.deparments.Where(d => d.DeparmentCode.Equals(code)).FirstOrDefaultAsync();
            return deparment;
        }

        public async Task<Deparment> GetDeparment(int deparmentId)
        {
            var deparment = await this.AppDbContext.deparments.FindAsync(deparmentId);
            return deparment;
        }

        public async Task<bool> DeparmentCodeExists(string code)
        {
            var response = await this.AppDbContext.deparments.Where(x => x.DeparmentCode.Equals(code)).FirstOrDefaultAsync();
            return response != null;
        }

        public async Task<List<PositionDto>> GetPositions()
        {
            var response = await (from pos in this.AppDbContext.positions
                                  select new PositionDto
                                  {
                                      Name = pos.Name,
                                      Code = pos.PositionCode,
                                      Id= pos.Id,
                                  }).ToListAsync();
            return response;
        }

        public async Task<Position> GetPosition(string positionCode)
        {
            var response = await this.AppDbContext.positions.Where(x => x.PositionCode.Equals(positionCode)).FirstOrDefaultAsync();
            return response;
        }

        public async Task<Position> GetPosition(int positionId)
        {
            var response = await this.AppDbContext.positions.FindAsync(positionId);
            return response;
        }

        public async Task<Position> AddPosition(PositionDto positionDto)
        {
            var position = new Position
            {
                Name= positionDto.Name,
                PositionCode    = positionDto.Code,
            };

            await this.AppDbContext.AddAsync(position);
            await this.AppDbContext.SaveChangesAsync();
            return position;
        }

        public async Task<Position> PutPosition(PositionDto positionDto)
        {
            var response = await this.AppDbContext.positions.FindAsync(positionDto.Id);
            response.PositionCode = positionDto.Code;
            response.Name = positionDto.Name;

            this.AppDbContext.Attach(response);
            this.AppDbContext.Entry(response).State = EntityState.Modified;
            await this.AppDbContext.SaveChangesAsync();
            return response;
        }

        public async Task<bool> PositionCodeExists(string code)
        {
            var response = await this.AppDbContext.positions.Where(x => x.PositionCode.Equals(code)).FirstOrDefaultAsync();
            return response != null;
        }

        public async Task<PositionListPaginatedDto> GetPositionPaginatedListDto(PositionListPaginatedDto request)
        {
            var response = new PositionListPaginatedDto();
            //si search es nulo o esta vacio, entonces hacemos un lista de empleados paginada sin ningun filtro

            response.Pagination = request.Pagination;

            if (string.IsNullOrEmpty(request.Search))
            {
               response.Positions = await(from pos in this.AppDbContext.positions
                                     select new PositionDto
                                     {
                                         Name = pos.Name,
                                         Code = pos.PositionCode,
                                         Id = pos.Id,
                                     }).OrderBy(p => p.Name).Pagination(response.Pagination).ToListAsync(); 

                response.Pagination.TotalRecords = this.AppDbContext.positions.Count();
            }
            else
            {
                var search = request.Search.ToUpper();

                response.Positions = await (from pos in this.AppDbContext.positions
                                                                 where pos.Name.ToUpper().StartsWith(search) ||
                                                                 pos.PositionCode.ToUpper().StartsWith(search)  
                                                                 select new PositionDto
                                                                 {
                                                                     Name = pos.Name,
                                                                     Code = pos.PositionCode,
                                                                     Id = pos.Id,
                                                                 }).OrderBy(p => p.Name).Pagination(response.Pagination)
                                                                 .ToListAsync();
                response.Pagination.TotalRecords = response.Positions.Count;

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

        public async Task<DeparmentListPaginatedDto> GetDeparmentListPaginatedDto(DeparmentListPaginatedDto request)
        {
            var response = new DeparmentListPaginatedDto();
            response.Pagination = request.Pagination;
            //si search es nulo o esta vacio, entonces hacemos un lista de empleados paginada sin ningun filtro
            if (string.IsNullOrEmpty(request.Search))
            {
                response.Deparments = await (from depts in this.AppDbContext.deparments
                                             join users in this.AppDbContext.AppUsers on depts.UserId equals users.Id
                                             select new DeparmentDto()
                                             {
                                                 Id = depts.Id,
                                                 Name = depts.Name,
                                                 Code = depts.DeparmentCode,
                                                 EmployeeCode = users.EmployeeCode,
                                                 FullName = $"{users.EmployeeCode}  {users.FirstName} {users.LastName}"
                                             }
                                             ).OrderBy(d => d.Name)
                                             .Pagination(response.Pagination)
                                             .ToListAsync();
                response.Pagination.TotalRecords = this.AppDbContext.deparments.Count();
            }
            else
            {
                var search = request.Search.ToUpper();

                response.Deparments = await (from depts in this.AppDbContext.deparments
                                             join users in this.AppDbContext.AppUsers on depts.UserId equals users.Id
                                             where depts.Name.ToUpper().StartsWith(search) ||
                                             depts.DeparmentCode.ToUpper().StartsWith(search)
                                             select new DeparmentDto()
                                             {
                                                 Id = depts.Id,
                                                 Name = depts.Name,
                                                 Code = depts.DeparmentCode,
                                                 EmployeeCode = users.EmployeeCode,
                                                 FullName = $"{users.EmployeeCode}  {users.FirstName} {users.LastName}"
                                             }
                                             ).OrderBy(d => d.Name)
                                             .Pagination(response.Pagination)
                                             .ToListAsync();

                response.Pagination.TotalRecords = this.AppDbContext.deparments.Count();

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

        public async Task<AreaListPaginatedDto> GetAreaListPaginatedDto(AreaListPaginatedDto request)
        {
            var response = new AreaListPaginatedDto();
            response.Pagination = request.Pagination;

            if (string.IsNullOrEmpty(request.Search))
            {
                response.Areas = await (from a in this.AppDbContext.areas
                                        join d in this.AppDbContext.deparments on a.DeparmentId equals d.Id
                                        let leaders = (from au in this.AppDbContext.areasUsers
                                                       join u in this.AppDbContext.AppUsers on au.UserId equals u.Id
                                                       where au.IsLeader && a.Id == au.AreaId
                                                       select new
                                                       {
                                                           FullName = $"{u.EmployeeCode} - {u.FirstName} {u.LastName}",
                                                           EmployeeCode = u.EmployeeCode,
                                                       }).FirstOrDefault()
                                        select new AreaDto()
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Code = a.AreaCode,
                                            DepartmentName = d.Name,
                                            EmployeeCode = leaders != null ? leaders.EmployeeCode : string.Empty,
                                            FullName = leaders != null ? leaders.FullName : "Sin director de area"
                                        }
                                        ).OrderBy(a => a.Name).Pagination(response.Pagination).ToListAsync();
            }
            else
            {

                var search = request.Search.ToUpper();
                response.Areas = await (from a in this.AppDbContext.areas
                                        join d in this.AppDbContext.deparments on a.DeparmentId equals d.Id
                                        where a.Name.ToUpper().StartsWith(search) || a.AreaCode.ToUpper().StartsWith(search)
                                        let leaders = (from au in this.AppDbContext.areasUsers
                                                       join u in this.AppDbContext.AppUsers on au.UserId equals u.Id
                                                       where au.IsLeader && a.Id == au.AreaId
                                                       select new
                                                       {
                                                           FullName = $"{u.EmployeeCode} - {u.FirstName} {u.LastName}",
                                                           EmployeeCode = u.EmployeeCode,
                                                       }).FirstOrDefault()
                                        select new AreaDto()
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Code = a.AreaCode,
                                            DepartmentName = d.Name,
                                            EmployeeCode = leaders != null ? leaders.EmployeeCode : string.Empty,
                                            FullName = leaders != null ? leaders.FullName : "Sin director de area"
                                        }
                                        ).OrderBy(a => a.Name).Pagination(response.Pagination).ToListAsync();
            }

            return response;

        }

        public async Task<List<string>> GetPositionBySearch(string search)
        {
            var response = new List<string>();
            if (!string.IsNullOrEmpty(search))
            {
                var searchUpper = search.ToUpper();
               response = await (from po in this.AppDbContext.positions
                                      where po.Name.ToUpper().StartsWith(searchUpper) ||
                                      po.Name.ToUpper().Contains(searchUpper) ||
                                      po.PositionCode.ToUpper().StartsWith(searchUpper) ||
                                      po.PositionCode.ToUpper().Contains(searchUpper)
                                      select $"{po.Name}"
                                     ).Take(10).ToListAsync();
            }
            else
            {
                response = await (from po in this.AppDbContext.positions
                                  select $"{po.Name}"
                                    ).Take(10).ToListAsync();
            }
            return response;
        }

        public async Task<DeparmentDto> GetDeparmentDto(int departmentId)
        {
            var response = await(from departs in this.AppDbContext.deparments
                                 join us in this.AppDbContext.Users on departs.UserId equals us.Id
                                 where departs.Id == departmentId
                                 select new DeparmentDto
                                 {
                                     Id = departs.Id,
                                     Name = departs.Name,
                                     Code = departs.DeparmentCode,
                                     EmployeeCode = us.EmployeeCode,
                                     FullName = $"{us.EmployeeCode} - {us.FirstName} {us.LastName}"
                                 }
                                   ).FirstOrDefaultAsync();
            return response;
        }

        public async Task<bool> DeletePosition(int id)
        {
            var areasUser = await this.AppDbContext.areasUsers.Where(x => x.PositionId == id).ToListAsync();
            if (areasUser != null && areasUser.Any())
                this.AppDbContext.RemoveRange(areasUser);

            var position = await this.GetPosition(id);
            this.AppDbContext.Remove(position);
            await this.AppDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAreas(int id)
        {
            var areasUser = await this.AppDbContext.areasUsers.Where(x => x.AreaId == id).ToListAsync();
            if(areasUser != null && areasUser.Any())
            this.AppDbContext.RemoveRange(areasUser);

            var area = await this.AppDbContext.areas.Where(x => x.Id == id).FirstOrDefaultAsync();

            this.AppDbContext.Remove(area);

            await this.AppDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var areaDepartmets = await this.AppDbContext.areas.Where(a => a.DeparmentId == id).ToListAsync();
            if (areaDepartmets != null && areaDepartmets.Any())
            {
                foreach (var area in areaDepartmets)
                {
                    await this.DeleteAreas(area.Id);
                }
            }

            var depto = await this.AppDbContext.deparments.Where(x => x.Id == id).FirstOrDefaultAsync();
            this.AppDbContext.Remove(depto);
            await this.AppDbContext.SaveChangesAsync();

            return true;
        }
    }
}
