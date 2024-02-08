using Attendance_Management.Domain;
using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.External;
using Attendance_Management.Domain.External.Models;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Repository
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IProviderDbContext providerDbContext;
        private readonly AppDbContext appDbContext;
		private readonly IEmployeeRepository employeeRepository;
		private readonly ExternalDbContext externalDbContext;
        public TransactionsRepository(IProviderDbContext providerDbContext, IEmployeeRepository employeeRepository, ExternalDbContext externalDbContext)
        {
            this.providerDbContext = providerDbContext;
            this.appDbContext = this.providerDbContext.GetAppDbContext();
			this.employeeRepository = employeeRepository;
			this.externalDbContext = this.providerDbContext.GetExternalDbContext();
        }

	
		

        public async Task<List<iclock_transactionToProcessDto>> GetTransactionsByRange(List<string> employeesCodes, DateTime initialDate, DateTime endDate)
        {
			endDate = endDate.AddDays(1);

            //var emps = this.externalDbContext.iclock_TransactionToProcessDtos
            //                .FromSqlInterpolated($"select * from public.\"GetEmployeesToProcessByRangeDates\"({employeesCodes}, {initialDate.Date.ToUniversalTime()}, {endDate.Date.ToUniversalTime()})")
            //                .IgnoreQueryFilters()
            //                .AsAsyncEnumerable();

			initialDate= initialDate.Date.ToUniversalTime();

			endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime();

            var punchesEmployees = await (from punches in this.externalDbContext.iclock_transaction
							  where punches.punch_time >= initialDate && punches.punch_time<= endDate
							  select new iclock_transactionToProcessDto
							  {
								  id= punches.id,
								  punchDate =punches.punch_time,
								  employeCode = punches.emp_code
							  }
                              ).ToListAsync();

            return punchesEmployees;
        }

        public async Task<iclock_transaction> AddIclock_Transaction(iclock_transactionDto request)
        {
           var copy = await this.externalDbContext.iclock_transaction.FirstAsync();

			var emp_Id = this.employeeRepository.GetEmployee(request.EmployeeCode).Result.id;

            var newEntity = new iclock_transaction()
			{
				punch_state = copy.punch_state,
				verify_type = copy.verify_type,
				work_code = copy.work_code,
				terminal_sn = copy.terminal_sn,
				terminal_alias = copy.terminal_alias,
				longitude = copy.longitude,
				latitude = copy.latitude,
				gps_location = copy.gps_location,
				mobile = copy.mobile,
				purpose = copy.purpose,
				crc = copy.crc,
				is_attendance = copy.is_attendance,
				reserved = copy.reserved,
				upload_time = copy.upload_time,
				sync_status = copy.sync_status,
				sync_time = copy.sync_time,
				emp_id = emp_Id,
				terminal_id = copy.terminal_id,
				is_mask = copy.is_mask,
				temperature = copy.temperature,
				emp_code = request.EmployeeCode,
				
			};

			newEntity.punch_time = Convert.ToDateTime(request.PunchDate).ToUniversalTime();

			await this.externalDbContext.AddAsync(newEntity);
            await this.externalDbContext.SaveChangesAsync();
			return newEntity;
        }

        public async Task<iclock_transaction> GetIclock_Transaction(int Id)
        {
			var response = await this.externalDbContext.iclock_transaction.FirstOrDefaultAsync(e => e.id == Id);
			return response;
        }

        public async Task<List<EmployeeDto>> GetEmployeesWithEmail()
        {

            var codes = await this.employeeRepository.GetAllEmployeCodes(true);

            var employeesWithEmail = new List<EmployeeDto>();


            foreach (var code in codes)
            {
                var employee = await (from emps in this.externalDbContext.personnel_employee
                                      where !string.IsNullOrEmpty(emps.email) && emps.emp_code.ToString() == code
                                      select new EmployeeDto
                                      {
                                          Id = emps.id,
                                          FirstName = emps.first_name,
                                          LastName = emps.last_name,
                                          EmployeeCode = emps.emp_code.ToString(),
                                          Email = emps.email,
                                      }).FirstOrDefaultAsync();

                if (employee != null)
                {
                    employeesWithEmail.Add(employee);

                }
            }


            return employeesWithEmail;
        }

        public List<EmployeeDto> FakeEmployees()
        {

			var response = new List<EmployeeDto>();


			var employee1 = new EmployeeDto()
			{
				EmployeeCode = "1020",
				FirstName = "Luis",
				LastName = "Lopez",
				Email = "202100135@upqroo.edu.mx",
				Id = 1,
			};

			response.Add(employee1);
			var employee2 = new EmployeeDto()
			{
				EmployeeCode = "1021",
				FirstName = "Irving",
				LastName = "Coyolt",
				Email = "202100106@upqroo.edu.mx",
				Id = 2,
			};
			response.Add(employee2);

			var employee3 = new EmployeeDto()
			{
				EmployeeCode = "1023",
				FirstName = "Breney",
				LastName = "Lopez",
				Email = "202100105@upqroo.edu.mx",
				Id = 3,
			};

			response.Add(employee3);

			return response;
        }

        public async Task<List<iclock_transactionToProcessDto>> GetPunchesOneDayForEmployeeCode(string date, string employeeCode)
        {
            var response = new List<iclock_transactionToProcessDto>();

            var dateForSearch = Convert.ToDateTime(date);

            var initialOfDay = dateForSearch.Date.ToUniversalTime();

            var finalOfDay = dateForSearch.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime();
            response = await (
                                from transac in this.externalDbContext.iclock_transaction
                                where (transac.punch_time >= initialOfDay &&
                                transac.punch_time <= finalOfDay) &&
                                transac.emp_code.Equals(employeeCode)
                                select new iclock_transactionToProcessDto
                                {
                                    id = transac.id,
                                    employeCode = employeeCode,
                                    punchDate = transac.punch_time,
                                }).ToListAsync();

            return response;
        }
    }
}
