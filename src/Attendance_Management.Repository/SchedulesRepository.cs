using Attendance_Management.Domain;
using Attendance_Management.Domain.Models;
using Attendance_Management.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Attendance_Management.Domain.DTOs;

namespace Attendance_Management.Repository
{
    public class SchedulesRepository : ISchedulesRepository
    {
        private readonly IProviderDbContext providerDbContext;
        private readonly AppDbContext appContext;
        public SchedulesRepository(IProviderDbContext providerDbContext)
        {
            this.providerDbContext = providerDbContext;
            this.appContext = this.providerDbContext.GetAppDbContext();
        }

        public async Task<List<Personnel_Schedules>> AddSchedule(List<Personnel_SchedulesDto> request)
        {
            //obtenemos el employeeCode, ya que es un dato que estaremos consultando seguido
            var employeeCode = request.First().EmployeeCode;

            //obtenemos los horarios del empleado si es que tiene 
            var existsSchedules =  await this.appContext.personnel_schedules.Where(s => s.EmployeeCode == employeeCode)
                                                                            .FirstOrDefaultAsync();


            //si el empleado tiene horarios debemos eliminarlos para asi insertar los que nos lleguen
            if (existsSchedules != null)
            {
               var personnelSchedules = this.appContext.personnel_schedules.Where(s  => s.EmployeeCode == employeeCode).ToList();

               this.appContext.personnel_schedules.RemoveRange(personnelSchedules);
            }

            var schedules = new List<Personnel_Schedules>();

            //creamos la lista de horarios que mandaremos a insertar 
            foreach (var schedule in request)
            {
                Personnel_Schedules personnel_Schedules = new()
                {
                    CheckInTime = schedule.CheckInTime,
                    CheckOutTime = schedule.CheckOutTime,
                    EmployeeCode = schedule.EmployeeCode,
                };

                personnel_Schedules.DayId = this.appContext.att_days.Where(d => d.Value == schedule.ValueDay).FirstOrDefaultAsync().Result.Id;

                schedules.Add(personnel_Schedules);
            }
            //insertamos la lista de empleados
            await this.appContext.personnel_schedules.AddRangeAsync(schedules);
           
            await this.appContext.SaveChangesAsync();
            return schedules;
        }

        public async Task<Att_Days> AddSDays(DayDto request)
        {
            var entity = new Att_Days()
            {
                Name = request.Name,
                Value = request.Value
            };
            await this.appContext.AddAsync(entity);
            await this.appContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Att_Days> GetDay(int Id)
        {
            var response = await this.appContext.att_days.FirstOrDefaultAsync(w => w.Id == Id);
            return response;
        }

        public async Task<Personnel_Schedules> GetSchedule(int Id)
        {
            var response = await this.appContext.personnel_schedules.FirstOrDefaultAsync(w => w.Id == Id);
            return response;
        }

        public async Task<List<Personnel_Schedules>> GetSchedules()
        {
            var response = await this.appContext.personnel_schedules.ToListAsync();
            return response;
        }

        public async Task<List<Personnel_Schedules>> GetSchedulesByEmployeeCode(string employeeCode)
        {
            var response = new List<Personnel_Schedules>();

            response = await this.appContext.personnel_schedules.Include(d => d.Day).Where(emp => emp.EmployeeCode == employeeCode).ToListAsync();
            return response;
        }

        //Este metodo devuelve los horarios haciendo un join con la entity Day
        public async Task<List<Personnel_Schedules>> GetSchedulesWithDay()
        {
            var response = await this.appContext.personnel_schedules.Include(d => d.Day).ToListAsync();
            return response;
        }
    }
}
