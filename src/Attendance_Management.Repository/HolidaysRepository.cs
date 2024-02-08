using Attendance_Management.Domain;
using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Extensions;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Repository
{
    public class HolidaysRepository : IHolidaysRepository
    {
        private readonly IProviderDbContext providerDbContext;
        private readonly AppDbContext appContext;
        private readonly IBusinessStructureRepository businessStructureRepository;
        public HolidaysRepository(IProviderDbContext providerDbContext, IBusinessStructureRepository businessStructureRepository)
        {
            this.providerDbContext = providerDbContext;
            this.appContext = this.providerDbContext.GetAppDbContext();
            this.businessStructureRepository = businessStructureRepository;
        }

        public async Task<List<Att_HolidaysDto>> GetHolidays(bool? isActive)
        {
            var response = await (
                                            from holiday in this.appContext.att_holidays
                                            join holidayEmployee in this.appContext.att_holidaysEmployees on holiday.Id equals holidayEmployee.HolidaysId into empGroup
                                            from holidayEmployee in empGroup.DefaultIfEmpty()  // Left Join
                                            join department in this.appContext.deparments on holidayEmployee.DepartmentId equals department.Id into deptGroup
                                            from department in deptGroup.DefaultIfEmpty()  // Left Join
                                            group department.Name by new
                                            {
                                                holiday.Id,
                                                holiday.Day,
                                                holiday.IsPartial,
                                                holiday.CheckIn,
                                                holiday.CheckOut,
                                                holiday.IsActive
                                            } into grouped
                                            select new Att_HolidaysDto
                                            {
                                                Id = grouped.Key.Id,
                                                Day = grouped.Key.Day,
                                                IsPartial = grouped.Key.IsPartial,
                                                CheckIn = (grouped.Key.CheckIn != null) ? grouped.Key.CheckIn.Value : null,
                                                CheckOut = (grouped.Key.CheckOut != null) ? grouped.Key.CheckOut.Value: null,
                                                IsActive = grouped.Key.IsActive,
                                                Departments = grouped.ToList()
                                            }
                                        ).ToListAsync();
            return response;
        }

        //public async Task<>

        public async Task<Att_Holidays> GetHoliday(int Id)
        {
            var response = await this.appContext.att_holidays.FirstOrDefaultAsync(h => h.Id == Id);
            return response;
        }
        public async Task<Att_Holidays> GetHoliday(DateTime day)
        {
            var response = await this.appContext.att_holidays
                                        .Where(holiday => holiday.Day == day)
                                        .FirstOrDefaultAsync();
            return response;
        }

        public async Task<List<Att_HolidaysEmployees>> GetHolidaysEmployees()
        {
            var response = await this.appContext.att_holidaysEmployees.ToListAsync();
            return response;
        }

        public async Task<Att_HolidaysEmployees> GetHolidayEmployee(int Id)
        {
            var response = await this.appContext.att_holidaysEmployees.FirstOrDefaultAsync(he => he.Id == Id);
            return response;
        }

        public async Task<Att_HolidaysEmployees> GetHolidaysEmployeesWithHolidayId(int Id)
        {
            var response = await this.appContext.att_holidaysEmployees.FirstOrDefaultAsync(he => he.HolidaysId == Id);
            return response;
        }
        
        public async Task<Att_HolidaysEmployees> GetHolidaysEmployeesWithDepartmentId(int Id)
        {
            var response = await this.appContext.att_holidaysEmployees.FirstOrDefaultAsync(he => he.DepartmentId == Id);
            return response;
        }

        public async Task<Att_Holidays> AddHolidays(Att_HolidaysDto request, Att_Holidays existingHoliday)
        {
            var response = new Att_Holidays();
            if (existingHoliday != null)
            {
                existingHoliday.Day = request.Day;
                existingHoliday.IsPartial = request.IsPartial;
                existingHoliday.CheckIn = request.CheckIn;
                existingHoliday.CheckOut = request.CheckOut;
                existingHoliday.IsActive = request.IsActive;

                var departments = await this.appContext.att_holidaysEmployees.Where(d => d.HolidaysId.Equals(existingHoliday.Id)).ToListAsync();

                this.appContext.RemoveRange(departments);

                foreach (var code in request.Departments)
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        var department = await this.businessStructureRepository.GetDeparment(code);
                        var holidaysEmployees = new Att_HolidaysEmployees()
                        {
                            HolidaysId = existingHoliday.Id,
                            DepartmentId = department.Id
                        };

                        await this.appContext.AddAsync(holidaysEmployees);
                    }
                }


                response = existingHoliday;
            }
            else
            {
                Att_Holidays holidays;
                if (!request.IsPartial)
                {
                    holidays = new Att_Holidays()
                    {
                        Day = request.Day,
                        IsPartial = request.IsPartial,
                        CheckIn = null,
                        CheckOut = null,
                        IsActive = request.IsActive
                    };
                }
                else
                {
                    holidays = new Att_Holidays()
                    {
                        Day = request.Day,
                        IsPartial = request.IsPartial,
                        CheckIn = request.CheckIn,
                        CheckOut = request.CheckOut,
                        IsActive = request.IsActive
                    };
                }
               

                await this.appContext.AddAsync(holidays);
                await this.appContext.SaveChangesAsync();

                foreach (var code in request.Departments)
                {
                    var department = await this.businessStructureRepository.GetDeparment(code);
                    var holidaysEmployees = new Att_HolidaysEmployees()
                    {
                        HolidaysId = holidays.Id,
                        DepartmentId = department.Id
                    };

                    await this.appContext.AddAsync(holidaysEmployees);
                }

                response = holidays;
            }

            await this.appContext.SaveChangesAsync();

            return response;
        }

        public async Task<Att_HolidaysEmployees> AddHolidaysEmployees(Att_HolidaysEmployeesDto request)
        {
            var holidaysEmployees = new Att_HolidaysEmployees()
            {
                HolidaysId = request.HolidaysId,
                DepartmentId = request.DepartmentId
            };

            await this.appContext.AddAsync(holidaysEmployees);

            return holidaysEmployees;
        }

        public async Task<HolidaysPaginatedListDto> GetListPaginatedHolidays(HolidaysPaginatedListDto request)
        {
            var response = new HolidaysPaginatedListDto();
            response.Pagination = request.Pagination;

            if (string.IsNullOrEmpty(request.Search))
            {
                response.Holidays = await (
                                            from holiday in this.appContext.att_holidays
                                            join holidayEmployee in this.appContext.att_holidaysEmployees on holiday.Id equals holidayEmployee.HolidaysId into empGroup
                                            from holidayEmployee in empGroup.DefaultIfEmpty()  // Left Join
                                            join department in this.appContext.deparments on holidayEmployee.DepartmentId equals department.Id into deptGroup
                                            from department in deptGroup.DefaultIfEmpty()  // Left Join
                                            group department.Name by new
                                            {
                                                holiday.Id,
                                                holiday.Day,
                                                holiday.IsPartial,
                                                holiday.CheckIn,
                                                holiday.CheckOut,
                                                holiday.IsActive
                                            } into grouped
                                            select new GetHolidaysDto
                                            {
                                                Id = grouped.Key.Id,
                                                Day = grouped.Key.Day.ToString("yyyy-MM-dd"),
                                                IsPartial = grouped.Key.IsPartial,
                                                CheckIn = (grouped.Key.CheckIn != null) ? grouped.Key.CheckIn.Value.ToString("HH:mm:ss") : null,
                                                CheckOut = (grouped.Key.CheckOut != null) ? grouped.Key.CheckOut.Value.ToString("HH:mm:ss") : null,
                                                IsActive = grouped.Key.IsActive,
                                                Departments = grouped.ToList()
                                            }
                                        ).Pagination(response.Pagination).ToListAsync();


            }
            else
            {

                var daySearch = Convert.ToDateTime(request.Search);
                response.Holidays = await (from holiday in this.appContext.att_holidays
                                           join holidayEmployee in this.appContext.att_holidaysEmployees on holiday.Id equals holidayEmployee.HolidaysId into empGroup
                                           from holidayEmployee in empGroup.DefaultIfEmpty()  // Left Join
                                           join department in this.appContext.deparments on holidayEmployee.DepartmentId equals department.Id into deptGroup
                                           from department in deptGroup.DefaultIfEmpty()  // Left Join
                                           where holiday.Day >= daySearch || holiday.Day <= daySearch 
                                           group department.Name by new
                                           {
                                               holiday.Id,
                                               holiday.Day,
                                               holiday.IsPartial,
                                               holiday.CheckIn,
                                               holiday.CheckOut,
                                               holiday.IsActive
                                           } into grouped
                                           select new GetHolidaysDto
                                           {
                                               Id = grouped.Key.Id,
                                               Day = grouped.Key.Day.ToString("yyyy-MM-dd"),
                                               IsPartial = grouped.Key.IsPartial,
                                               CheckIn = (grouped.Key.CheckIn != null) ? grouped.Key.CheckIn.Value.ToString("HH:mm:ss") : null,
                                               CheckOut = (grouped.Key.CheckOut != null) ? grouped.Key.CheckOut.Value.ToString("HH:mm:ss") : null,
                                               IsActive = grouped.Key.IsActive,
                                               Departments = grouped.ToList()
                                           }
                                           ).Pagination(response.Pagination).ToListAsync();
            }

            return response;
        }

        public async Task<GetHolidaysDto> GetHolidayDto(int Id)
        {

            var holidayDto = await (from holiday in this.appContext.att_holidays
                                    where holiday.Id == Id
                                    select new GetHolidaysDto
                                    {
                                      Id = holiday.Id,
                                      Day = holiday.Day.ToString("yyyy-MM-dd"),
                                      IsPartial =  holiday.IsPartial,
                                      CheckIn = holiday.CheckIn != null ? holiday.CheckIn.Value.ToString("HH:mm:ss") : null,
                                      CheckOut = holiday.CheckOut != null ? holiday.CheckOut.Value.ToString("HH:mm:ss") : null,
                                      IsActive = holiday.IsActive
                                    }).FirstOrDefaultAsync();

            holidayDto.Departments = await (from dh in this.appContext.att_holidaysEmployees
                                            join d in this.appContext.deparments on dh.DepartmentId equals d.Id
                                            where dh.HolidaysId == holidayDto.Id
                                            select d.DeparmentCode
                                            ).ToListAsync();

            return holidayDto;
            
        }

        public async Task<Att_Holidays> GetHolidayPartialByEmpCode(string day, string empcode)
        {
            var dayToSearch = Convert.ToDateTime(day);

            var holidayPartial = await (from h in this.appContext.att_holidays
                                        join hd in this.appContext.att_holidaysEmployees on h.Id equals hd.HolidaysId
                                        join d in this.appContext.deparments on hd.DepartmentId equals d.Id
                                        join a in this.appContext.areas on d.Id equals a.DeparmentId
                                        join au in this.appContext.areasUsers on a.Id equals au.AreaId
                                        join u in this.appContext.AppUsers on au.UserId equals u.Id
                                        where u.EmployeeCode.Equals(empcode) && h.IsPartial && 
                                        hd.DepartmentId == a.DeparmentId && h.Day.Date.Equals(dayToSearch.Date)
                                        select h
                                        ).FirstOrDefaultAsync();
            return holidayPartial;

        }

        public async Task<bool> DeleteHoliday(int Id)
        {
            var holiday = await this.appContext.att_holidays.Where(x => x.Id == Id).FirstOrDefaultAsync();
            this.appContext.Remove(holiday);
            await this.appContext.SaveChangesAsync();
            return true;
        }
    }
}
