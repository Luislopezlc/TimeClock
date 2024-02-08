using Attendance_Management.Domain;
using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Extensions;
using Attendance_Management.Domain.External;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.External.Models;
using Attendance_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Attendance_Management.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IProviderDbContext providerDbContext;
        private readonly ExternalDbContext externalDbContext;
        private readonly AppDbContext appDbContext;
        public EmployeeRepository(IProviderDbContext providerDbContext)
        {
            this.providerDbContext = providerDbContext;
            this.externalDbContext = this.providerDbContext.GetExternalDbContext();
            this.appDbContext = this.providerDbContext.GetAppDbContext();
        }
        public async Task<List<EmployeeDto>> GetEmployees()
        {
            var employees = await (from emps in this.externalDbContext.personnel_employee
                                   select new EmployeeDto
                                   {
                                       EmployeeCode = emps.emp_code.ToString(),
                                       Email = emps.email,
                                       FirstName = emps.first_name,
                                       LastName = emps.last_name,
                                   }).ToListAsync();

            return employees;
        }

        public async Task<Personnel_employee> GetEmployee(string empCode)
        {
            var employee = await this.externalDbContext.personnel_employee
                                                  .FirstOrDefaultAsync(e => e.emp_code == Convert.ToInt32(empCode));

            return employee;
        }

        public async Task<string> GetPositionNameOfEmployee(string empCode)
        {
            var employee = await GetEmployee(empCode);

            var position = new Personnel_position(); 
            if(employee != null && employee.position_id != null)
            {
                position = await this.externalDbContext.personnel_position
                                                 .FirstOrDefaultAsync(p => p.id == employee.position_id);
            }
            else
            {
                position.position_name = "Sin puesto";
            }
             

            return position.position_name;
        }

        public async Task<Personnel_employee> AddEmployee(EmployeeDto request)
        {
            var employee = await this.GetEmployee("595");

            var date = DateTime.Now.ToUniversalTime();

            var newEmploye = new Personnel_employee()
            {
                create_time = date,
                create_user = employee.create_user,
                change_time = date,
                change_user = employee.change_user,
                status = employee.status,
                emp_code = Convert.ToInt32(request.EmployeeCode),
                first_name = request.FirstName,
                last_name = request.LastName,
                nickname = employee.nickname,
                passport = employee.passport,
                driver_license_automobile = employee.driver_license_automobile,
                driver_license_motorcycle = employee.driver_license_motorcycle,
                photo = employee.photo,
                self_password = employee.self_password,
                device_password = employee.device_password,
                dev_privilege = employee.dev_privilege,
                card_no = employee.card_no,
                acc_group = employee.acc_group,
                acc_timezone = employee.acc_timezone,
                gender = employee.gender,
                birthday = date,
                address = employee.address,
                postcode = employee.postcode,
                office_tel = employee.office_tel,
                contact_tel = employee.contact_tel,
                mobile = employee.mobile,
                national_num = employee.national_num,
                payroll_num = employee.payroll_num,
                internal_emp_num = employee.internal_emp_num,
                national = employee.national,
                religion = employee.religion,
                title = employee.title,
                enroll_sn = employee.enroll_sn,
                ssn = employee.ssn,
                update_time = date,
                hire_date = date,
                verify_mode = employee.verify_mode,
                city = employee.city,
                is_admin = employee.is_admin,
                emp_type = employee.emp_type,
                enable_att = employee.enable_att,
                enable_payroll = employee.enable_payroll,
                enable_overtime = employee.enable_overtime,
                enable_holiday = employee.enable_holiday,
                deleted = employee.deleted,
                reserved = employee.reserved,
                del_tag = employee.del_tag,
                app_status = employee.app_status,
                app_role = employee.app_role,
                email = request.Email,
                last_login = date,
                is_active = employee.is_active,
                vacation_rule = employee.vacation_rule,
                company_id = employee.company_id,
                department_id = employee.department_id,
                position_id = employee.position_id,
            };

            await this.externalDbContext.AddAsync(newEmploye);
            await this.externalDbContext.SaveChangesAsync();
            return newEmploye;
        }


        public async Task<EmployeesPaginatedListDto> EmployeesPaginatedList(EmployeesPaginatedListWithFiltres request)
        {

            var employees = new List<EmployeeDto>();

            //si search es nulo o esta vacio, entonces hacemos un lista de empleados paginada sin ningun filtro
            if (string.IsNullOrEmpty(request.Search))
            {
                employees = await (from emps in this.externalDbContext.personnel_employee.OrderBy(e => e.first_name).Pagination(request.Pagination)
                                   join jobs in this.externalDbContext.personnel_position on
                                   emps.position_id equals jobs.id into empJobs
                                   from job in empJobs.DefaultIfEmpty() 
                                   select new EmployeeDto
                                   {
                                       Id = emps.id,
                                       FirstName = emps.first_name,
                                       LastName = emps.last_name,
                                       Email = emps.email,
                                       EmployeeCode = emps.emp_code.ToString(),
                                       Job = job != null ? job.position_name : "Sin puesto",

                                   }).ToListAsync();
                request.Pagination.TotalRecords = this.externalDbContext.personnel_employee.Count();

            }
            else //sino es nulo entonces hacemos una lista de empleados filtrando por el search
            {
                var empCodeRequest = string.Empty;

                int index = request.Search.IndexOf('-');

                if (index != -1)
                {
                    empCodeRequest = request.Search.Substring(0, index).Trim();
                }

                //hacemos a todo mayusculas el texto que llego y le quitamos los espacios en blanco si tuviera
                var search = request.Search.ToUpper().Trim();

                //obtenemos el primer caracter para saber si es leta o digito
                var firstChar = request.Search[0];

                //si es letra filtramos por nombre o apellido
                if (char.IsLetter(firstChar))
                {
                    employees = await (from emps in this.externalDbContext.personnel_employee
                                       join jobs in this.externalDbContext.personnel_position on
                                       emps.position_id equals jobs.id into empJobs
                                       from job in empJobs.DefaultIfEmpty()
                                       where emps.first_name.ToUpper().Trim().StartsWith(search) ||
                                      emps.last_name.ToUpper().Trim().StartsWith(search)
                                       select new EmployeeDto
                                       {
                                           Id = emps.id,
                                           FirstName = emps.first_name,
                                           LastName = emps.last_name,
                                           Email = emps.email,
                                           EmployeeCode = emps.emp_code.ToString(),
                                           Job = job != null ? job.position_name : "Sin puesto",

                                       }).Pagination(request.Pagination).ToListAsync();
                }
                else if (char.IsDigit(firstChar))//si es numero buscamos employeeCode
                {
                    employees = await (from emps in this.externalDbContext.personnel_employee
                                       join jobs in this.externalDbContext.personnel_position on
                                       emps.position_id equals jobs.id into empJobs
                                       from job in empJobs.DefaultIfEmpty()
                                       where emps.emp_code.ToString().Trim().StartsWith(search) ||
                                       emps.emp_code.ToString().Trim().Contains(search) ||
                                       emps.emp_code.ToString().Trim().Equals(empCodeRequest)
                                       select new EmployeeDto
                                       {
                                           Id = emps.id,
                                           FirstName = emps.first_name,
                                           LastName = emps.last_name,
                                           Email = emps.email,
                                           EmployeeCode = emps.emp_code.ToString(),
                                           Job = job != null ? job.position_name : "Sin puesto",

                                       }).Pagination(request.Pagination).ToListAsync();
                }


                request.Pagination.TotalRecords = employees.Count;

            }


            var employeesWithSchedules = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var schedules = await this.appDbContext.personnel_schedules.Include(x => x.Day).Where(sche => sche.EmployeeCode == employee.EmployeeCode).ToListAsync();

                var date = DateTime.Now.ToShortDateString();

                schedules.ForEach(she => she.CheckInTime = Convert.ToDateTime(date + " " + she.CheckInTime).ToString("HH:mm"));
                schedules.ForEach(she => she.CheckOutTime = Convert.ToDateTime(date + " " + she.CheckOutTime).ToString("HH:mm"));

                employee.Schedules = schedules;
                employeesWithSchedules.Add(employee);
            }

            var response = new EmployeesPaginatedListDto();
            response.Employees = employeesWithSchedules;


            double dividend = Convert.ToDouble(request.Pagination.TotalRecords);
            double divisor = Convert.ToDouble(request.Pagination.RecordsForPage);

            //10.3
            double result = dividend / divisor;
            //10
            var resultWithoutDecimals = Math.Floor(result);

            if (resultWithoutDecimals != result)
            {
                request.Pagination.TotalPages = Convert.ToInt32(resultWithoutDecimals) + 1;
            }
            else
            {
               request.Pagination.TotalPages = Convert.ToInt32(result);
            }


            response.Pagination = request.Pagination;

            return response;
        }

        public async Task<List<EmployeeDto>> GetEmployeeSearchBar(string request)
        {
            var response = new List<EmployeeDto>();
            var firstChar = request[0];


            if (char.IsLetter(firstChar))
            {
                response = await (from emps in this.externalDbContext.personnel_employee
                                  where emps.first_name.ToUpper().Trim().StartsWith(request.ToUpper().Trim()) ||
                                  emps.last_name.ToUpper().Trim().StartsWith(request.ToUpper().Trim())
                                  select new EmployeeDto
                                  {
                                    EmployeeCode = emps.emp_code.ToString(),
                                    FirstName = emps.first_name,
                                    LastName = emps.last_name,
                                  }
                                  ).Take(10).ToListAsync();
            }
            else if (char.IsDigit(firstChar))
            {
                

                if(request.Contains("-"))
                {
                    int index = request.IndexOf('-');

                    if (index != -1)
                    {
                        request = request.Substring(0, index).Trim();
                    }
                }

                response = await (from emps in this.externalDbContext.personnel_employee
                                  where emps.emp_code.ToString().StartsWith(request)
                                  select new EmployeeDto
                                  {
                                      EmployeeCode = emps.emp_code.ToString(),
                                      FirstName = emps.first_name,
                                      LastName = emps.last_name,
                                  }
                                   ).Take(10).ToListAsync();
            }

            return response;

        }

        public async Task<List<string>> GetAllEmployeCodes(bool IsForIncidentsReport)
        {
            var response = new List<string>();

            if(IsForIncidentsReport)
            {
                //retorna solo los emp_codes que tiene horario
               response = await this.appDbContext.personnel_schedules.Select(e => e.EmployeeCode).Distinct().ToListAsync();
            }
            else
            {
                //retorna todos los emp_codes existentes
                response = await this.externalDbContext.personnel_employee.Select(e => e.emp_code.ToString()).Distinct().ToListAsync();
            }

            return response;
        }

        public async Task<EmployeeDto> GetEmployeeDto(string empCode)
        {
            var employeeCode = Convert.ToInt32(empCode);

            var response = await (from emps in this.externalDbContext.personnel_employee
                                  where emps.emp_code == employeeCode
                                  select  new EmployeeDto
                                  {
                                      Id = emps.id,
                                      EmployeeCode = emps.emp_code.ToString(),
                                      Email = emps.email,
                                      FirstName = emps.first_name,
                                      LastName = emps.last_name,
                                      Job =  this.GetPositionNameOfEmployee(empCode.ToString()).Result,
                                  }
                                  ).FirstOrDefaultAsync();
            return response;
        }

        public async Task<int?> GetEmpCodeByName(string Name, string Lastname)
        {
            var response = await this.externalDbContext.personnel_employee
                .Where(e => e.first_name.ToLower().Contains(Name.ToLower())
                         || e.last_name.ToLower().Contains(Lastname.ToLower()))
                .FirstOrDefaultAsync();

            return response?.emp_code;
        }
    }
}
