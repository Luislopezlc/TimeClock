using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Extensions;
using Attendance_Management.Domain.External.Models;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using static Humanizer.In;
using static Humanizer.On;

namespace Attendance_Management.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository transactionsRepository;
        private readonly ISchedulesService schedulesService;
        private readonly IEmployeeService employeeService;
        private readonly IHolidaysService holidayService;
        private readonly ITemplateService templateService;
        private readonly ILogger<TransactionsService> logger;

        public TransactionsService(ITransactionsRepository transactionsRepository,
            ISchedulesService schedulesService, IEmployeeService employeeService, ITemplateService templateService, ILogger<TransactionsService> logger, IHolidaysService holidayService)
        {
            this.transactionsRepository = transactionsRepository;
            this.schedulesService = schedulesService;
            this.templateService = templateService;
            this.employeeService = employeeService;
            this.holidayService = holidayService;
            this.logger = logger;
        }

        public async Task<ResponseDto> AddIclock_Transaction(iclock_transactionDto request)
        {
            var response = new ResponseDto();
            try
            {
                response.Payload = await this.transactionsRepository.AddIclock_Transaction(request);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<ResponseDto> AddIclock_Transactions(List<iclock_transactionDto> request)
        {
            var response = new ResponseDto();

            try
            {
                foreach (var trasaction in request)
                {
                    await this.AddIclock_Transaction(trasaction);
                }
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<ResponseDto> GetEmployeesWithEmail()
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.transactionsRepository.GetEmployeesWithEmail();
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }
            return response;
        }

        public async Task<ResponseDto> GetIclock_Transaction(int Id)
        {
            var response = new ResponseDto();

            try
            {
                response.Payload = await this.transactionsRepository.GetIclock_Transaction(Id);
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<ResponseDto> GetIncidentReport(IncidentReportRequestDto request)
        {
            var response = new ResponseDto();

            try
            {
                //Verifica que la fecha final sea mayor a la fecha inicial, si hay una incongruencia en las fechas retornara un null
                if (Convert.ToDateTime(request.EndDate) < Convert.ToDateTime(request.InitialDate))
                {
                    response.Issuccessful = false;
                    response.Status = 400;
                    response.Errors = ErrorMessages.AddMessageError("InitialDate", Messages.InitialDateError);
                    return response;
                }

                //este es el objeto que se devolvera y que se estara construyendo
                var incidentReport = new IncidentReportDto();

                incidentReport.InitialDate = request.InitialDate;
                incidentReport.EndDate = request.EndDate;

                //el primer paso es convertir los parametros initialDate y EndDate a tipo DateTime Utc
                var initialDateUtc = Convert.ToDateTime(request.InitialDate).ToUniversalTime();
                var endDateUtc = Convert.ToDateTime(request.EndDate).ToUniversalTime();

                //el siguiente paso es obtener una lista de los dias que se van a calcular incidencias
                List<string> daysToCalculateIncident = new();
                for (var date = initialDateUtc; date <= endDateUtc; date = date.AddDays(1))
                {
                    daysToCalculateIncident.Add(date.ToShortDateString());
                }
                //creamos la lista que contendra los employeeCodes
                var employeesCode = new ResponseDto();

                //si search es nulo o string.empty entonces vamos a obtener todos los empleados
                if (string.IsNullOrEmpty(request.Search))
                {
                    //se obtienen los codigos de los empleados que tienen horarios
                    employeesCode = await this.employeeService.GetAllEmployeCodes(true);
                }
                else
                {
                    //obtenemos los employeeCodes a traves del search
                    employeesCode = await this.employeeService.GetEmployeeCodesBySearch(request.Search);
                }

                //Si lo que contiene payload es una lista de strings verificaremos si esta lista no contiene ningún elemento
                if (employeesCode.Payload is List<string> listEmployeesCode)
                {
                    employeesCode.Payload = listEmployeesCode;
                    if (listEmployeesCode.Count == 0)
                    {
                        return response;
                    }

                    //se va a iterar por empleado y luego se iterara por dia
                    //se hizo de esta manera para poder generar las listas que tiene cada empleado
                    var incidentReportEmp = new EmployeeIncidentReportDto();

                    foreach (var employee in listEmployeesCode)
                    {
                        var incidentsReportEmployee = await this.GetIncidentsReportbyEmployeeCode(employee, daysToCalculateIncident);
                        if (!incidentsReportEmployee.Issuccessful)
                        {
                            return incidentsReportEmployee;
                        }

                        if (incidentsReportEmployee != null)
                        {
                            if (incidentsReportEmployee.Payload is EmployeeIncidentReportDto incidentsReportEmployees)
                            {
                                incidentReportEmp = incidentsReportEmployees;
                                incidentReport.Employees.Add(incidentReportEmp);

                            }
                        }
                    }
                }
                else
                {
                    //Aquí debería ir algún error?
                }


                request.Pagination.TotalRecords = incidentReport.Employees.Count;

                //agregamos la paginacion de esta forma:
                //primero ordenamos la list por el nombre del empleado y luego le aplicamos la paginacion
                incidentReport.Employees = incidentReport.Employees.AsQueryable().OrderBy(emp => emp.FirstName).Pagination(request.Pagination).ToList();

                double dividend = Convert.ToDouble(request.Pagination.TotalRecords);
                double divisor = Convert.ToDouble(request.Pagination.RecordsForPage);
                if (divisor <= 0)
                {
                    divisor = 25;
                }
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

                incidentReport.Pagination = request.Pagination;

                response.Issuccessful = true;
                response.Payload = incidentReport;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<ResponseDto> GetIncidentsIndividualReport(string employee, string initialDate, string endDate)
        {
            var response = new ResponseDto();

            try
            {
                //este es el objeto que se devolvera y que se estara construyendo
                var incidentReport = new IncidentsIndividualReportEmployeeDto();

                incidentReport.InitialDate = initialDate;
                incidentReport.EndDate = endDate;

                //el primer paso es convertir los parametros initialDate y EndDate a tipo DateTime Utc
                var initialDateUtc = Convert.ToDateTime(initialDate).ToUniversalTime();
                var endDateUtc = Convert.ToDateTime(endDate).ToUniversalTime();

                if (initialDateUtc > endDateUtc)
                {
                    response.Issuccessful = false;
                    response.Errors = ErrorMessages.AddMessageError("Fecha inicial", Messages.InitialDateError);
                    response.Status = 400;
                    return response;
                }



                //el siguiente paso es obtener una lista de los dias que se van a calcular incidencias
                List<string> daysToCalculateIncident = new();
                for (var date = initialDateUtc; date <= endDateUtc; date = date.AddDays(1))
                {
                    daysToCalculateIncident.Add(date.ToShortDateString());
                }

                var employeeReportResponse = await this.GetIncidentsReportbyEmployeeCode(employee, daysToCalculateIncident);
                if (!employeeReportResponse.Issuccessful)
                {
                    return employeeReportResponse;
                }

                var employeeReport = new EmployeeIncidentReportDto();

                if (employeeReportResponse.Payload is EmployeeIncidentReportDto employeesReport)
                {
                    employeeReport = employeesReport;
                }

                if (employeeReport != null)
                {
                    incidentReport.EmployeeIncidentsReport = employeeReport;
                }

                response.Issuccessful = true;
                response.Payload = incidentReport;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        //este metodo debe recibir los parametros startDate y endDate como DateTimeUTC
        public async Task<ResponseDto> GetTransactionsByRange(List<string> employeesCodes, DateTime startDate, DateTime endDate)
        {
            var response = new ResponseDto();

            try
            {
                var transactionByRange = await this.transactionsRepository.GetTransactionsByRange(employeesCodes, startDate, endDate);
                TimeZoneInfo timeZoneGMT_5 = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time (Mexico)");
                transactionByRange.ForEach(x => x.punchDate = TimeZoneInfo.ConvertTimeFromUtc(x.punchDate, timeZoneGMT_5));
                response.Payload = transactionByRange;
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        //este metodo envia correos los dias 1 y 16 del mes, los correos contienen informacion de los periodos de incidencias 
        // el dia 1 del mes se deben de mandar las incidencias de la segunda quincena del mes anterior, ejemplo:
        //Hoy es 1 de agosto, se deben de mandar las incidencias del 16 de julio al 31 de julio
        //el dia 16 del mes se deben de mandar las incidencias de la primera quincena de ese de mes, ejemplo:
        //Hoy el 16 de agosto, se deben de mandar las incidencias del 1 de agosto al 15 de agosto
        public async Task<ResponseDto> SendIncidentsByEmail(IncidenctsReportEmailDto request, ConfigurationSMPT configurationSMPT)
        {

            var response = new ResponseDto();
            var mailCounting = new MailCountDto();
            try
            {
                //este opcion nos sirve para mandar un correo en la fecha que nosotros definamos y a algunos enpleados
                if (request != null)
                {

                    //fecha de inicio para hacer el reporte las incidencias
                    var initialDateForIncidentsReport = Convert.ToDateTime(request.InitialDate);


                    //FECHA fin para hacer el reporte de las incidencias
                    var endDateForIncidentsReport = Convert.ToDateTime(request.EndDate);

                    var initialDate = initialDateForIncidentsReport.ToShortDateString();
                    var endDate = endDateForIncidentsReport.ToShortDateString();

                    if (initialDateForIncidentsReport > endDateForIncidentsReport)
                    {
                        response.Issuccessful = false;
                        response.Status = 400;
                        response.Errors = ErrorMessages.AddMessageError("InitialDate", Messages.InitialDateError);
                        return response;
                    }

                    //si hay por lo menos un empleado en la configuracion de la peticion se manda solo a el
                    if (request.EmployeeCodes != null && request.EmployeeCodes.Count > 0)
                    {
                        foreach (var empcode in request.EmployeeCodes)
                        {
                            var reportIndividualResponse = await this.GetIncidentsIndividualReport(empcode, initialDate, endDate);
                            var reportIndividual = new IncidentsIndividualReportEmployeeDto();
                            if (reportIndividualResponse.Payload is IncidentsIndividualReportEmployeeDto incidentsIndividualReport)
                            {
                                reportIndividual = incidentsIndividualReport;
                            }

                            var responseEmployee = await this.employeeService.GetEmployeeDto(empcode);
                            var employee = new EmployeeDto();
                            if (responseEmployee.Issuccessful)
                            {
                                if (responseEmployee.Payload is EmployeeDto emp)
                                    employee = emp;
                            }
                            //si el reporte si almenos una falta o un retardo menor entonces si envia el correo
                            if (reportIndividual != null)
                            {
                                //se envia el correo
                                //aqui se debe poner el metodo que manda el correo pasandole como parametro
                                // reportIndividual 
                                if (employee.Email != null)
                                {
                                    var sent = await sendEmail(reportIndividual, employee, configurationSMPT);
                                    if (sent)
                                    {
                                        mailCounting.EmailsSent++;
                                        mailCounting.EmployeeCodes.Add(employee.EmployeeCode);
                                    }
                                }
                                Thread.Sleep(5000);

                            }
                        }

                        response.Payload = null;
                        response.Issuccessful = true;

                        return response;
                    }
                    else
                    {
                        //obtenemos los empleados que tienen cargados sus horarios y emails

                        var employeesResponse = await this.GetEmployeesWithEmail();
                        if (!employeesResponse.Issuccessful)
                        {
                            return employeesResponse;
                        }
                        var employees = new List<EmployeeDto>();
                        if (employeesResponse.Payload is List<EmployeeDto> employeesWithEmail)
                        {
                            employees = employeesWithEmail;
                        }

                        //vamos a recorrer cada empleado para armar su reporte de incidencias a cada uno
                        foreach (var employee in employees)
                        {
                            var reportIndividualResponse = await this.GetIncidentsIndividualReport(employee.EmployeeCode, initialDate, endDate);
                            var reportIndividual = new IncidentsIndividualReportEmployeeDto();
                            if (reportIndividualResponse.Payload is IncidentsIndividualReportEmployeeDto incidentsIndividualReport)
                            {
                                reportIndividual = incidentsIndividualReport;
                            }

                            //si el reporte si almenos una falta o un retardo menor entonces si envia el correo
                            if (reportIndividual != null)
                            {
                                //se envia el correo
                                //aqui se debe poner el metodo que manda el correo pasandole como parametro
                                // reportIndividual
                                if (employee.Email != null)
                                {
                                    var sent = await sendEmail(reportIndividual, employee, configurationSMPT);
                                    if (sent)
                                    {
                                        mailCounting.EmailsSent++;
                                        mailCounting.EmployeeCodes.Add(employee.EmployeeCode);
                                    }
                                }


                                Thread.Sleep(5000);
                            }
                        }
                    }


                }
                else
                {

                    //obtenemos la fecha del dia actual en formato utc

                    var currentDay = DateTime.UtcNow;

                    //convertimos la fecha utc a la zona horaria de CanCun, QuintanaRoo
                    TimeZoneInfo timeZoneGMT_5 = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time (Mexico)");
                    DateTime currentDayGMT_5 = TimeZoneInfo.ConvertTimeFromUtc(currentDay, timeZoneGMT_5);

                    //obtenemos el dia en int en el que estamos
                    int dia = currentDayGMT_5.Day;

                    //son el mismo proceso solo varian el rango de dias en que se hace el calculo 
                    //de las incidencias dependiendo de si es 1 o 16
                    //arriba en el nombre del metodo estan las reglas para los periodos 
                    if (dia == 1)
                    {
                        //obtenemos el mes actual y se le resta uno para hacer el reporte de la segunda mitad del mes anterior
                        var currentMouth = 0;
                        if (currentDayGMT_5.Month == 1)
                        {
                            currentMouth = 12;
                        }
                        else
                        {
                            currentMouth = currentDayGMT_5.Month - 1;
                        }
                        //obtenemos el año actual
                        var currentYear = currentDayGMT_5.Year;
                        //dia de inicio de la segunda quincena
                        var startDayOfTheSecondFortnight = 16;

                        //fecha de inicio para hacer el reporte las incidencias
                        var initialDateForIncidentsReport = new DateTime(currentYear, currentMouth, startDayOfTheSecondFortnight);

                        //obtenemos el ultimo dia del mes, nos servira como ultimo dia de quincena
                        var endDayOfTheSecondFortnight = DateTime.DaysInMonth(currentYear, currentMouth);

                        //FECHA fin para hacer el reporte de las incidencias
                        var endDateForIncidentsReport = new DateTime(currentYear, currentMouth, endDayOfTheSecondFortnight);

                        //obtenemos los empleados que tienen cargados sus horarios y emails
                        var employeesResponse = await this.GetEmployeesWithEmail();
                        if (!employeesResponse.Issuccessful)
                        {
                            return employeesResponse;
                        }
                        var employees = new List<EmployeeDto>();
                        if (employeesResponse.Payload is List<EmployeeDto> employeesWithEmail)
                        {
                            employees = employeesWithEmail;
                        }

                        var initialDate = initialDateForIncidentsReport.ToShortDateString();
                        var endDate = endDateForIncidentsReport.ToShortDateString();

                        //vamos a recorrer cada empleado para armar su reporte de incidencias a cada uno
                        foreach (var employee in employees)
                        {
                            var reportIndividualResponse = await this.GetIncidentsIndividualReport(employee.EmployeeCode, initialDate, endDate);
                            var reportIndividual = new IncidentsIndividualReportEmployeeDto();
                            if (reportIndividualResponse.Payload is IncidentsIndividualReportEmployeeDto incidentsIndividualReport)
                            {
                                reportIndividual = incidentsIndividualReport;
                            }

                            //si el reporte si almenos una falta o un retardo menor entonces si envia el correo
                            if (reportIndividual != null)
                            {
                                //se envia el correo
                                //aqui se debe poner el metodo que manda el correo pasandole como parametro
                                // reportIndividual 
                                if (employee.Email != null)
                                {
                                    var sent = await sendEmail(reportIndividual, employee, configurationSMPT);
                                    if (sent)
                                    {
                                        mailCounting.EmailsSent++;
                                        mailCounting.EmployeeCodes.Add(employee.EmployeeCode);
                                    }
                                }
                                Thread.Sleep(5000);

                            }
                        }

                    }
                    else if (dia == 16)
                    {
                        //obtenemos el mes actual 
                        var currentMouth = currentDayGMT_5.Month;
                        //obtenemos el año actual
                        var currentYear = currentDayGMT_5.Year;
                        //dia de inicio de la primera quicena
                        var startDayOfTheFirstFortnight = 1;

                        //fecha de inicio para hacer el reporte las incidencias
                        var initialDateForIncidentsReport = new DateTime(currentYear, currentMouth, startDayOfTheFirstFortnight);

                        //dia final para la primera quincena del mes
                        var endDayOfTheFirstFortnight = 15;


                        //FECHA fin para hacer el reporte de las incidencias
                        var endDateForIncidentsReport = new DateTime(currentYear, currentMouth, endDayOfTheFirstFortnight);

                        //obtenemos los empleados que tienen cargados sus horarios y emails
                        var employeesResponse = await this.GetEmployeesWithEmail();
                        var employees = new List<EmployeeDto>();
                        if (employeesResponse.Payload is List<EmployeeDto> employeesWithEmail)
                        {
                            employees = employeesWithEmail;
                        }
                        var initialDate = initialDateForIncidentsReport.ToShortDateString();
                        var endDate = endDateForIncidentsReport.ToShortDateString();

                        //vamos a recorrer cada empleado para armar su reporte de incidencias a cada uno
                        foreach (var employee in employees)
                        {
                            var reportIndividualResponse = await this.GetIncidentsIndividualReport(employee.EmployeeCode, initialDate, endDate);
                            var reportIndividual = new IncidentsIndividualReportEmployeeDto();
                            if (reportIndividualResponse.Payload is IncidentsIndividualReportEmployeeDto incidentsIndividualReport)
                            {
                                reportIndividual = incidentsIndividualReport;
                            }
                            //si el reporte si almenos una falta o un retardo menor entonces si envia el correo 
                            if (reportIndividual != null)
                            {
                                //se envia el correo
                                //aqui se debe poner el metodo que manda el correo pasandole como parametro
                                // reportIndividual 

                                if (employee.Email != null)
                                {
                                    var sent = await sendEmail(reportIndividual, employee, configurationSMPT);
                                    if (sent)
                                    {
                                        mailCounting.EmailsSent++;
                                        mailCounting.EmployeeCodes.Add(employee.EmployeeCode);
                                    }
                                }
                                Thread.Sleep(5000);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
                return response;
            }

            response.Payload = mailCounting;
            response.Issuccessful = true;

            return response;
        }

        //
        public async Task<ResponseDto> GetIncidentsToday()
        {
            var response = new ResponseDto();
            try
            {
                //Se declara el tipo Absence y Delay
                var absence = "A";
                var delay = "D";

                //Generamos una lista vacia para las incidencias de hoy
                List<IncidentToday> incidentsTodayList = new List<IncidentToday>();

                //Obtenemos la fecha y hora actual en UTC
                var now = DateTime.UtcNow;
                //Obtenemos la fecha actual para tomarla como la fecha inicial y se le agregan 5 horas que son las que hay de diferencia con el UTC
                var today = DateTime.Today.AddHours(5);


                var incidentReport = new IncidentReportDto();

                incidentReport.InitialDate = today.ToString();
                incidentReport.EndDate = now.ToString();

                var minutesForAssistance = 10; //cantidad de minutos despues de la hora de entrada para contar como asistencia
                var minutesForDelay = 11; //cantidad de minutos que tiene para que despues de su hora de entrada cuente como retardo menor
                var minutesForDelayMax = 20; //cantidad de minutos maximos para que su entrada cuente como retardo menor
                var minutesForLongerDelay = 21; //cantidad de minutos para que su check in cuente como retardo mayor-falta

                string dateToCalculateIncident = today.ToShortDateString();

                ////se obtiene los horarios dados de alta en la db de postgres
                var schedulesResponse = await this.schedulesService.GetSchedulesWithDay();

                if (!schedulesResponse.Issuccessful)
                {
                    return schedulesResponse;
                }

                var schedules = new List<Personnel_Schedules>();
                if (schedulesResponse.Payload is List<Personnel_Schedules> schedule)
                {
                    schedules = schedule;
                }


                //se obtienen los codigos de los empleados que tienen horarios
                var employeesCode = schedules.Select(s => s.EmployeeCode).Distinct().ToList();

                // se obtiene las checadas solo de los empleados que tienen horarios  y se les manda los codigos de empleados 
                var employeePunchesResponse = await this.GetTransactionsByRange(employeesCode, today, now);
                if (!employeePunchesResponse.Issuccessful)
                {
                    return employeePunchesResponse;
                }
                var employeePunches = new List<iclock_transactionToProcessDto>();
                if (employeePunchesResponse.Payload is List<iclock_transactionToProcessDto> employeePunch)
                {
                    employeePunches = employeePunch;
                }

                //Se iterara por empleado
                foreach (var employee in employeesCode)
                {
                    var employeeForIncidentReportResponse = await this.employeeService.GetEmployeeIncidentReport(employee);
                    if (!employeeForIncidentReportResponse.Issuccessful)
                    {
                        return employeeForIncidentReportResponse;
                    }
                    var employeeForIncidentReport = new EmployeeIncidentReportDto();
                    if (employeeForIncidentReportResponse.Payload is EmployeeIncidentReportDto employeeReport)
                    {
                        employeeForIncidentReport = employeeReport;
                    }

                    //Incializando los contadores de incidencias
                    employeeForIncidentReport.MinorDelays = 0;
                    employeeForIncidentReport.Absences = 0;
                    employeeForIncidentReport.LongerDelays = 0;

                    var dayOfWeek = (int)Convert.ToDateTime(dateToCalculateIncident).DayOfWeek;

                    //Se consulta si necesita checar para este día dependiendo del valor del día
                    var requiresAssistanceForThisDay = schedules.Where(s => s.Day.Value == dayOfWeek
                                                                        && s.EmployeeCode == employee
                                                                        ).Any();

                    if (requiresAssistanceForThisDay)
                    {
                        //Obtenemos las checadas que tuvo el empleado durante el día que estamos procesando
                        var punches = employeePunches.Where(p =>
                                                            p.punchDate.ToShortDateString()
                                                                        .Equals(dateToCalculateIncident)
                                                            && p.employeCode == employee
                                                            ).ToList();
                        //Obtenemos la cantidad de checadas que tuvo en el día a procesar
                        var numberOfCheck = punches.Count;
                        //Si el numero de checada es mayor o igual a dos se debe de procesar
                        //Nota: Pueden ser mas de dos checadas por dia ya que el reloj esta posicionado en un lugar donde pasan 
                        //muchos empleados y pueden marcar una checada accidental

                        //Si el número de checadas es mayor o igual a 2 entonces se procesa
                        if (numberOfCheck >= 2)
                        {
                            //Fecha con hora de entrada del día + los minutos para la entrada cuente como asistencia
                            //Esta fecha es despues de su hora de entrada
                            //Se le suma un minuto y se le resta un segundo para que valide las checadas en el último
                            //Segundo antes de que cuente como retardo menor
                            var checkInTimeToCompereAfter = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                .Where(s => s.EmployeeCode == employee
                                                                                && s.Day.Value == dayOfWeek)
                                                                                .Select(s => s.CheckInTime)
                                                                                .First())
                                                                                .AddMinutes(minutesForAssistance + 1)
                                                                                .AddSeconds(-1);

                            //Fecha para comparar si llego antes de su hora de entrada
                            //Ahora comparamos desde el inico del día
                            var checkInTImeToCompareBefore = Convert.ToDateTime(dateToCalculateIncident);

                            //Fecha para comprar su salida, esta fecha es exactamente su hora de salida
                            var checkOutTimeToCompareAfter = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                .Where(s => s.EmployeeCode == employee
                                                                                && s.Day.Value == dayOfWeek)
                                                                                .Select(s => s.CheckOutTime)
                                                                                .First());
                            //Fecha limite para checar su salida, puede checar su salida en el ultimo minuto del día
                            //se le añade un día y se le resta un segundo para que pueda validar el último minuto del día
                            var checkOutTimeToCompareBefore = Convert.ToDateTime(dateToCalculateIncident)
                                                                        .AddDays(1)
                                                                        .AddSeconds(-1);

                            //Obtenemos la primera checada del día antes de la hora máxima de entrada
                            var checkInTime = punches.Where(p => p.punchDate >= checkInTImeToCompareBefore
                                                            && p.punchDate <= checkInTimeToCompereAfter)
                                                        .OrderBy(p => p.punchDate)
                                                        .FirstOrDefault();
                            //Obtenemos la última checada del día despues de su hora de salida minima
                            var checkOutTime = punches.Where(p => p.punchDate >= checkOutTimeToCompareAfter
                                                            && p.punchDate <= checkOutTimeToCompareBefore)
                                                        .OrderByDescending(p => p.punchDate)
                                                        .FirstOrDefault();

                            if (checkOutTime == null)//Si no hay hora de salida valida es falta injustificada
                            {
                                //aumenta a 1 el número de faltas
                                employeeForIncidentReport.Absences++;

                                var employeeWithEmpCodeResponse = await this.employeeService.GetEmployeeNameWithEmpCode(employee);
                                if (!employeeWithEmpCodeResponse.Issuccessful)
                                {
                                    return employeeWithEmpCodeResponse;
                                }
                                string employeeWithEmpCode = "";
                                if (employeeWithEmpCodeResponse.Payload is string employeeEmpCode)
                                {
                                    employeeWithEmpCode = employeeEmpCode;
                                }

                                //Añadimos al empleado a la lista de incidencias de hoy
                                incidentsTodayList.Add(new IncidentToday
                                {
                                    Id = int.Parse(employee),
                                    Employee = employeeWithEmpCode,
                                    Type = absence
                                });
                            }
                            else
                            {
                                //Si es nulo se trata de un retardo hay que verificar de que tipo es (Retardo menor o retardo mayor)
                                if (checkInTime == null)
                                {
                                    //Retardo menor
                                    //Fecha minima para que su entrada cuente como retardo menor
                                    var checkInTimeWithDelayMinorMin = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                .Where(s => s.EmployeeCode == employee
                                                                                && s.Day.Value == dayOfWeek)
                                                                                .Select(s => s.CheckInTime)
                                                                                .First())
                                                                                .AddMinutes(minutesForDelay);
                                    //Fecha máxima para su entrada cuente como retardo menor
                                    //Se le suma 1 minuto y se le resta un segundo para que valide el último segundo del minuto máximo
                                    var checkInTimeWithDelayMinorMax = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                .Where(s => s.EmployeeCode == employee
                                                                                && s.Day.Value == dayOfWeek)
                                                                                .Select(s => s.CheckInTime)
                                                                                .First())
                                                                                .AddMinutes(minutesForDelayMax + 1)
                                                                                .AddSeconds(-1);
                                    //Obtenemos la primera checada entre el tiempo permitido para que sea retardo menor
                                    var checkInTimeWithDelatMinor = punches.Where(p =>
                                                                                    p.punchDate >= checkInTimeWithDelayMinorMin
                                                                                && p.punchDate <= checkInTimeWithDelayMinorMax
                                                                                )
                                                                            .OrderBy(p => p.punchDate)
                                                                            .FirstOrDefault();
                                    //Si es diferente de nulo se trata de un retardo menor
                                    if (checkInTimeWithDelatMinor != null)
                                    {
                                        //Aumenta a 1 el número de retardos menores
                                        employeeForIncidentReport.MinorDelays++;

                                        var employeeWithEmpCodeResponse = await this.employeeService.GetEmployeeNameWithEmpCode(employee);
                                        if (!employeeWithEmpCodeResponse.Issuccessful)
                                        {
                                            return employeeWithEmpCodeResponse;
                                        }
                                        string employeeWithEmpCode = "";
                                        if (employeeWithEmpCodeResponse.Payload is string employeeEmpCode)
                                        {
                                            employeeWithEmpCode = employeeEmpCode;
                                        }
                                        //Añadimos al empleado a la lista de incidencias de hoy
                                        incidentsTodayList.Add(new IncidentToday
                                        {
                                            Id = int.Parse(employee),
                                            Employee = employeeWithEmpCode,
                                            Type = delay
                                        });
                                    }
                                    else//Si es nulo, hay 2 posibilidades: retardo mayor o que todas las checadas sean despues de la hora de entrada
                                    {
                                        //Fecha minima para que su entrada cuente como retardo mayor-falta
                                        var checkInTimeWithLongerDelayMin = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                    .Where(s => s.EmployeeCode == employee
                                                                                    && s.Day.Value == dayOfWeek)
                                                                                    .Select(s => s.CheckInTime)
                                                                                    .First())
                                                                                    .AddMinutes(minutesForLongerDelay);
                                        //fecha minima para que la entrada cuente como entrada, es el ultimo segundo antes de la hora de salida
                                        var checkInTimeWithLongerDelayMax = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                    .Where(s => s.EmployeeCode == employee
                                                                                    && s.Day.Value == dayOfWeek)
                                                                                    .Select(s => s.CheckOutTime)
                                                                                    .First())
                                                                                    .AddSeconds(-1);
                                        //obtenemos la primera checada que contaria como retardo mayor-falta
                                        var checkInTimeWithLongerDelay = punches.Where(p => p.punchDate >= checkInTimeWithLongerDelayMin
                                                                                    && p.punchDate <= checkInTimeWithLongerDelayMax
                                                                                ).OrderBy(p => p.punchDate)
                                                                                 .FirstOrDefault();
                                        //si es diferente a nulo entonces se trata de un retardo mayor
                                        if (checkInTimeWithLongerDelay != null)
                                        {
                                            //aumenta 1 al numero de retardos mayores
                                            employeeForIncidentReport.LongerDelays++;
                                        }
                                        else//si es nulo, entonces todos los registros del dia son despues de la hora de entrada = falta
                                        {
                                            //aumenta 1 al numero de faltas
                                            employeeForIncidentReport.Absences++;
                                        }

                                    }
                                }
                            }
                        }
                        else if (numberOfCheck == 1) //En esta condición hay 2 posibilidades, si se ejecuta durante su jornada de trabajo quiere decir que registro una entrada,
                                                     //el segundo es si se ejecuta despues de su jornada lo que indicaría que salió muy temprano o no registro su salida
                        {
                            //Aquí se obtiene la fecha junto con la hora a la que tiene que entrar hoy
                            var checkInTimeToday = Convert.ToDateTime(dateToCalculateIncident + " " + schedules.Where(s => s.EmployeeCode == employee && s.Day.Value == dayOfWeek).Select(s => s.CheckInTime).First());

                            //La condición aquí es si la hora es mayor a la hora a la que tenía que entrar, si se cumple quiere decir que ya debio registrar su entrada
                            if (now > checkInTimeToday)
                            {
                                //Fecha con hora de entrada del día + los minutos para la entrada cuente como asistencia
                                //Esta fecha es despues de su hora de entrada
                                //Se le suma un minuto y se le resta un segundo para que valide las checadas en el último
                                //Segundo antes de que cuente como retardo menor
                                var checkInTimeToCompereAfter = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                    .Where(s => s.EmployeeCode == employee
                                                                                    && s.Day.Value == dayOfWeek)
                                                                                    .Select(s => s.CheckInTime)
                                                                                    .First())
                                                                                    .AddMinutes(minutesForAssistance + 1)
                                                                                    .AddSeconds(-1);

                                //Fecha para comparar si llego antes de su hora de entrada
                                //Ahora comparamos desde el inico del día
                                var checkInTImeToCompareBefore = Convert.ToDateTime(dateToCalculateIncident);

                                //Obtenemos la primera checada del día antes de la hora máxima de entrada
                                var checkInTime = punches.Where(p => p.punchDate >= checkInTImeToCompareBefore
                                                                && p.punchDate <= checkInTimeToCompereAfter)
                                                            .OrderBy(p => p.punchDate)
                                                            .FirstOrDefault();

                                //Si es nulo se trata de un retardo, hay que verificar cual es
                                if (checkInTime == null)
                                {
                                    //Retardo menor
                                    //Fecha minima para que su entrada cuente como retardo menor
                                    var checkInTimeWithDelayMinorMin = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                .Where(s => s.EmployeeCode == employee
                                                                                && s.Day.Value == dayOfWeek)
                                                                                .Select(s => s.CheckInTime)
                                                                                .First())
                                                                                .AddMinutes(minutesForDelay);
                                    //Fecha máxima para su entrada cuente como retardo menor
                                    //Se le suma 1 minuto y se le resta un segundo para que valide el último segundo del minuto máximo
                                    var checkInTimeWithDelayMinorMax = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                .Where(s => s.EmployeeCode == employee
                                                                                && s.Day.Value == dayOfWeek)
                                                                                .Select(s => s.CheckInTime)
                                                                                .First())
                                                                                .AddMinutes(minutesForDelayMax + 1)
                                                                                .AddSeconds(-1);
                                    //Obtenemos la primera checada entre el tiempo permitido para que sea retardo menor
                                    var checkInTimeWithDelayMinor = punches.Where(p =>
                                                                                    p.punchDate >= checkInTimeWithDelayMinorMin
                                                                                && p.punchDate <= checkInTimeWithDelayMinorMax
                                                                                )
                                                                            .OrderBy(p => p.punchDate)
                                                                            .FirstOrDefault();
                                    //Si es diferente de nulo se trata de un retardo menor
                                    if (checkInTimeWithDelayMinor != null)
                                    {
                                        //Aumenta a 1 el número de retardos menores
                                        employeeForIncidentReport.MinorDelays++;

                                        var employeeWithEmpCodeResponse = await this.employeeService.GetEmployeeNameWithEmpCode(employee);
                                        if (!employeeWithEmpCodeResponse.Issuccessful)
                                        {
                                            return employeeWithEmpCodeResponse;
                                        }
                                        string employeeWithEmpCode = "";
                                        if (employeeWithEmpCodeResponse.Payload is string employeeEmpCode)
                                        {
                                            employeeWithEmpCode = employeeEmpCode;
                                        }
                                        //Añadimos al empleado a la lista de incidencias de hoy
                                        incidentsTodayList.Add(new IncidentToday
                                        {
                                            Id = int.Parse(employee),
                                            Employee = employeeWithEmpCode,
                                            Type = delay
                                        });

                                    }
                                    else//Si es nulo, hay 2 posibilidades: retardo mayor o que todas las checadas sean despues de la hora de entrada
                                    {
                                        //Fecha minima para que su entrada cuente como retardo mayor-falta
                                        var checkInTimeWithLongerDelayMin = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                    .Where(s => s.EmployeeCode == employee
                                                                                    && s.Day.Value == dayOfWeek)
                                                                                    .Select(s => s.CheckInTime)
                                                                                    .First())
                                                                                    .AddMinutes(minutesForLongerDelay);
                                        //fecha minima para que la entrada cuente como entrada, es el ultimo segundo antes de la hora de salida
                                        var checkInTimeWithLongerDelayMax = Convert.ToDateTime(dateToCalculateIncident + " " + schedules
                                                                                    .Where(s => s.EmployeeCode == employee
                                                                                    && s.Day.Value == dayOfWeek)
                                                                                    .Select(s => s.CheckOutTime)
                                                                                    .First())
                                                                                    .AddSeconds(-1);
                                        //obtenemos la primera checada que contaria como retardo mayor-falta
                                        var checkInTimeWithLongerDelay = punches.Where(p => p.punchDate >= checkInTimeWithLongerDelayMin
                                                                                    && p.punchDate <= checkInTimeWithLongerDelayMax
                                                                                ).OrderBy(p => p.punchDate)
                                                                                 .FirstOrDefault();
                                        //si es diferente a nulo entonces se trata de un retardo mayor
                                        if (checkInTimeWithLongerDelay != null)
                                        {
                                            //aumenta 1 al numero de retardos mayores
                                            employeeForIncidentReport.LongerDelays++;

                                            var employeeWithEmpCodeResponse = await this.employeeService.GetEmployeeNameWithEmpCode(employee);
                                            if (!employeeWithEmpCodeResponse.Issuccessful)
                                            {
                                                return employeeWithEmpCodeResponse;
                                            }
                                            string employeeWithEmpCode = "";
                                            if (employeeWithEmpCodeResponse.Payload is string employeeEmpCode)
                                            {
                                                employeeWithEmpCode = employeeEmpCode;
                                            }
                                            //Añadimos al empleado a la lista de incidencias de hoy
                                            incidentsTodayList.Add(new IncidentToday
                                            {
                                                Id = int.Parse(employee),
                                                Employee = employeeWithEmpCode,
                                                Type = absence
                                            });
                                        }
                                        else//si es nulo, entonces todos los registros del dia son despues de la hora de entrada = falta
                                        {
                                            //aumenta 1 al numero de faltas
                                            employeeForIncidentReport.Absences++;

                                            var employeeWithEmpCodeResponse = await this.employeeService.GetEmployeeNameWithEmpCode(employee);
                                            if (!employeeWithEmpCodeResponse.Issuccessful)
                                            {
                                                return employeeWithEmpCodeResponse;
                                            }
                                            string employeeWithEmpCode = "";
                                            if (employeeWithEmpCodeResponse.Payload is string employeeEmpCode)
                                            {
                                                employeeWithEmpCode = employeeEmpCode;
                                            }
                                            //Añadimos al empleado a la lista de incidencias de hoy
                                            incidentsTodayList.Add(new IncidentToday
                                            {
                                                Id = int.Parse(employee),
                                                Employee = employeeWithEmpCode,
                                                Type = absence
                                            });
                                        }
                                    }
                                }
                            }

                        }
                        else //Si no hay checadas registras entonces se tienen 2 opciones, la priimera es sí ya finalizo el día entonce se le considera como falta
                        {
                            //Aquí se obtiene la fecha junto con la hora a la que tiene que salir hoy
                            var checkOutTimeToday = Convert.ToDateTime(dateToCalculateIncident + " " + schedules.Where(s => s.EmployeeCode == employee && s.Day.Value == dayOfWeek).Select(s => s.CheckOutTime).First());
                            //checkOutTimeToday.AddHours(5);

                            //Si la hora en la que se ejecuta es despues de la hora de salida entonces se le considera falta, de lo contrario entonces aún no le corresponde checar
                            if (now > checkOutTimeToday)
                            {
                                //aumenta 1 al numero de faltas
                                employeeForIncidentReport.Absences++;

                                var employeeWithEmpCodeResponse = await this.employeeService.GetEmployeeNameWithEmpCode(employee);
                                if (!employeeWithEmpCodeResponse.Issuccessful)
                                {
                                    return employeeWithEmpCodeResponse;
                                }
                                string employeeWithEmpCode = "";
                                if (employeeWithEmpCodeResponse.Payload is string employeeEmpCode)
                                {
                                    employeeWithEmpCode = employeeEmpCode;
                                }
                                //Añadimos al empleado a la lista de incidencias de hoy
                                incidentsTodayList.Add(new IncidentToday
                                {
                                    Id = int.Parse(employee),
                                    Employee = employeeWithEmpCode,
                                    Type = absence
                                });
                            }
                        }

                    }
                }

                response.Issuccessful = true;
                response.Payload = incidentsTodayList;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }

        public async Task<bool> sendEmail(IncidentsIndividualReportEmployeeDto incidentReportDtp, EmployeeDto employee, ConfigurationSMPT configurationSMPT)
        {
            string from = configurationSMPT.Email;
            string displayName = configurationSMPT.DisplayName;

            try
            {
                SmtpClient client = new SmtpClient(configurationSMPT.Host, configurationSMPT.Port);

                client.Credentials = new NetworkCredential(from, configurationSMPT.Password);
                client.EnableSsl = true;

                MailMessage email = new MailMessage();
                email.From = new MailAddress(from, displayName);

                var htmlBody = await CustomBody(incidentReportDtp, configurationSMPT.PathEmail);

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html);
                string imagePathF = @"wwwroot/img/upqroo_logo.png";//aqui la ruta de tu imagen
                LinkedResource logo = new LinkedResource(imagePathF, MediaTypeNames.Image.Jpeg);
                logo.ContentId = "logo";
                //var pathFile = @"Files/solicitudDePermiso.xls";
                //email.Attachments.Add(new Attachment(pathFile));  asi se hacia anteriormente no borrar por si falla el link de drive
                htmlView.LinkedResources.Add(logo);
                email.AlternateViews.Add(htmlView);

                if (configurationSMPT.IsTest)
                {
                    email.To.Add(configurationSMPT.EmailTest);
                }
                else
                {
                    email.To.Add(employee.Email);
                }

                email.Subject = $"Incidencias {incidentReportDtp.InitialDate} - {incidentReportDtp.EndDate}";
                email.Body = htmlBody;
                email.IsBodyHtml = true;

                client.Send(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }


        public async Task<string> CustomBody(IncidentsIndividualReportEmployeeDto IncidentsIndividualReportEmployee, string pathFile)
        {
            var employeeIncidentReport = IncidentsIndividualReportEmployee.EmployeeIncidentsReport;

            var absenceListDates = dateListTable(employeeIncidentReport.AbsenceDates);
            var minorListDates = dateListTable(employeeIncidentReport.MinorDelayDates);
            var longerListDates = dateListTable(employeeIncidentReport.LongerDelaysDates);
            var absenceWithoutListDates = dateListTable(employeeIncidentReport.AbsenceDatesWithoutExit);

            var initialDate = IncidentsIndividualReportEmployee.InitialDate;
            var endDate = IncidentsIndividualReportEmployee.EndDate;

            var body = await this.templateService.GetIncidentsReportEmailTemplate(pathFile);

            var bodyFirstName = body.Replace("{employeeIncidentReport.FirstName}", employeeIncidentReport.FirstName);
            var bodyInitialDate = bodyFirstName.Replace("{initialDate}", IncidentsIndividualReportEmployee.InitialDate);
            var bodyEndDate = bodyInitialDate.Replace("{endDate}", IncidentsIndividualReportEmployee.EndDate);
            var bodyFullName = bodyEndDate.Replace("{employeeIncidentReport.FullName}", employeeIncidentReport.FullName);
            var bodyAbsence = bodyFullName.Replace("{employeeIncidentReport.Absences}", employeeIncidentReport.Absences.ToString());
            var bodyAbsenceList = bodyAbsence.Replace("{absenceListDates}", absenceListDates);
            var bodyAbsenceWithoutListDates = bodyAbsenceList.Replace("{absenceWithoutListDates}", absenceWithoutListDates);
            var bodyMirorDelays = bodyAbsenceWithoutListDates.Replace("{employeeIncidentReport.MinorDelays}", employeeIncidentReport.MinorDelays.ToString());
            var bodyMinorListDates = bodyMirorDelays.Replace("{minorListDates}", minorListDates);
            var bodyLongerDelays = bodyMinorListDates.Replace("{employeeIncidentReport.LongerDelays}", employeeIncidentReport.LongerDelays.ToString());
            var bodyLongerListDates = bodyLongerDelays.Replace("{longerListDates}", longerListDates);
            var bodyTotalIncidents = bodyLongerListDates.Replace("{employeeIncidentReport.TotalIncidents}", employeeIncidentReport.TotalIncidents.ToString());


            return bodyTotalIncidents;
        }

        public string dateListTable(List<string> dates)
        {
            var dateList = "";

            foreach (var date in dates)
            {
                dateList += $"<p>{date}</p>";
            }
            return dateList;
        }

        public async Task<ResponseDto> GetPunchesOneDayForEmployeeCode(string date, string employeeCode)
        {
            var response = new ResponseDto();

            try
            {
                var responsePunches = await this.transactionsRepository.GetPunchesOneDayForEmployeeCode(date, employeeCode);
                TimeZoneInfo timeZoneGMT_5 = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time (Mexico)");
                responsePunches.ForEach(x => x.punchDate = TimeZoneInfo.ConvertTimeFromUtc(x.punchDate, timeZoneGMT_5));
                response.Payload = responsePunches;
                response.Issuccessful = true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }
        private async Task<ResponseDto> GetIncidentsReportbyEmployeeCode(string employee, List<string> daysToCalculateIncident)
        {
            var response = new ResponseDto();

            try
            {
                var employeeForReport = await this.employeeService.GetEmployeeIncidentReport(employee);
                if (!employeeForReport.Issuccessful)
                {
                    return employeeForReport;
                }
                var employeeForIncidentReport = new EmployeeIncidentReportDto();

                if (employeeForReport.Payload is EmployeeIncidentReportDto employeeReport)
                {
                    employeeForIncidentReport = employeeReport;
                }

                //inicializando contadores de incidencias
                employeeForIncidentReport.MinorDelays = 0;
                employeeForIncidentReport.Absences = 0;
                employeeForIncidentReport.LongerDelays = 0;

                //var minutesTolerance = 30; //cantidad de minutos que tiene para checar salida o entrada dependiendo del caso * para los reportes de PA
                var minutesForDelay = 11; //cantidad de minutos que tiene para que despues de su hora de entrada cuente como retardo menor
                var minutesForDelayMax = 20; //cantidad de minutos maximos para que su entrada cuente como retardo menor
                var minutesForLongerDelay = 21; //cantidad de minutos para que su check in cuente como retardo mayor-falta

                var schedulesForReport = await this.schedulesService.GetSchedulesByEmployeeCode(employee);
                if (!schedulesForReport.Issuccessful)
                {
                    return schedulesForReport;
                }
                var schedules = new List<Personnel_Schedules>();

                //Verificar si schedulesForReport.Payload es una Lista de Personnel_Schedules y asignarla
                //a schedules para poder utilizarla en el resto del codigo
                if (schedulesForReport.Payload is List<Personnel_Schedules> schedulesReport)
                {
                    schedules = schedulesReport;
                }

                foreach (var day in daysToCalculateIncident)
                {
                    //obtenemos el valor del dia a procesar
                    var dayOfWeek = (int)Convert.ToDateTime(day).DayOfWeek;

                    //se consulta si necesita checar para este dia dependiendo del valor del dia 
                    var requiresAssistanceForThisDay = schedules.Where(s => s.Day.Value == dayOfWeek
                                                                        && s.EmployeeCode == employee
                                                                        ).Any();

                    //se valida un dai inhabil general
                    var holidayResponse = await this.holidayService.GetHolidays(true);

                    var holiday = (List<Att_HolidaysDto>)holidayResponse.Payload;

                    var isHoliday = holiday.Where(h => h.Day == Convert.ToDateTime(day) && h.IsPartial != true).Any();


                    if (requiresAssistanceForThisDay && !isHoliday)
                    {
                        //obtenemos las checadas que tuvo el empleado en el dia que estamos procesando
                        var punchesResponse = await this.GetPunchesOneDayForEmployeeCode(day, employee);
                        if (!punchesResponse.Issuccessful)
                        {
                            return punchesResponse;
                        }
                        var punches = new List<iclock_transactionToProcessDto>();
                        if (punchesResponse.Payload is List<iclock_transactionToProcessDto> punch)
                        {
                            punches = punch;
                        }


                        //obtenemos la cantidad de checadas que tuvo en el dia a procesar

                        var numberOfChecks = punches.Count;
                        //si el numero de checadas es mayor o igual a dos se debe de procesar
                        //Nota: Pueden ser mas de dos checadas por dia ya que el reloj esta posicionado en un lugar donde pasan 
                        //muchos empleados y pueden marcar una checada accidental

                        // si el numero de checadas es mayor o igual a 2 entonces se procesa
                        if (numberOfChecks >= 2)
                        {
                            //obtenemos la primera checada del dia antes de la hora maxima de entrada 
                            var checkInTime = punches.OrderBy(p => p.punchDate)
                                                        .First().punchDate;
                            //obtenemos la ultima checada del dia despues de su hora de salida minima 
                            var checkOutTime = punches.OrderByDescending(p => p.punchDate)
                                                      .First().punchDate;

                            DateTime schedulesCheckIn = new();
                            DateTime schedulesCheckOut = new();
                            Att_Holidays holidayPartial = new();

                            //se valida un diaInhabil parcial

                            var existsHolidayPartial = await this.holidayService.GetHolidayPartialByEmpCode(day, employee);

                            if (existsHolidayPartial.Status != 200)
                            {
                                return existsHolidayPartial;
                            }
                            else
                            {
                                holidayPartial = existsHolidayPartial.Payload as Att_Holidays;
                            }


                            if (holidayPartial != null && holidayPartial.CheckIn != null)
                            {
                                schedulesCheckIn = holidayPartial.CheckIn.Value;
                            }
                            else
                            {
                                schedulesCheckIn = Convert.ToDateTime(day + " " + schedules
                                              .Where(s => s.EmployeeCode == employee
                                              && s.Day.Value == dayOfWeek)
                                              .Select(s => s.CheckInTime)
                                              .First());
                            }

                            if (holidayPartial != null && holidayPartial.CheckOut != null)
                            {
                                schedulesCheckOut = holidayPartial.CheckOut.Value;
                            }
                            else
                            {
                                schedulesCheckOut = Convert.ToDateTime(day + " " + schedules
                                              .Where(s => s.EmployeeCode == employee
                                              && s.Day.Value == dayOfWeek)
                                              .Select(s => s.CheckOutTime)
                                              .First());
                            }


                            var delayCheckInTime = checkInTime - schedulesCheckIn;

                            var delayCheckOut = checkOutTime - schedulesCheckOut;


                            if (delayCheckOut.TotalMinutes >= 0.0)
                            {
                                if (((int)delayCheckInTime.TotalMinutes) >= minutesForDelay &&
                                    ((int)delayCheckInTime.TotalMinutes) <= minutesForDelayMax)
                                {
                                    //aumenta 1 al numero de retardos menores
                                    employeeForIncidentReport.MinorDelays++;

                                    //le damos el formato que se necesita en la vista: Lun 01/01/23 8:31 am se muestra la hora en la que llego ese dia
                                    var dayDelay = checkInTime.ToString("ddd dd/MM/yy h:mm tt");

                                    //se le da el formato a la hora de llegada para mostrar en horario debio llegar 

                                    var checkInTimeMinorDelay = schedulesCheckIn.ToString("h:mm tt");

                                    //se agrega a lista de retardos menores
                                    employeeForIncidentReport.MinorDelayDates.Add($"{dayDelay} -> {checkInTimeMinorDelay}");
                                }
                                else if (((int)delayCheckInTime.TotalMinutes) >= minutesForLongerDelay)
                                {
                                    //aumenta 1 al numero de retardos mayores
                                    employeeForIncidentReport.LongerDelays++;

                                    //le damos el formato que se necesita en la vista: Lun 01/01/23 8:31 am se muestra la hora en la que llego ese dia
                                    var dayLongerDelay = checkInTime.ToString("ddd dd/MM/yy h:mm tt");

                                    //se obtiene la hora de entrada exacta del empleado

                                    //se le da el formato a la hora de llegada para mostrar en horario debio llegar 
                                    var checkInTimeDelayLonger = schedulesCheckIn.ToString("h:mm tt");
                                    //se agrega a lista de retardos mayores
                                    employeeForIncidentReport.LongerDelaysDates.Add($"{dayLongerDelay} -> {checkInTimeDelayLonger}");
                                }
                            }
                            else
                            {
                                //sino hay hora de salida valida es falta injustificada (se fue antes de la hora de salida)
                                //aumenta 1 al numero de faltas
                                employeeForIncidentReport.Absences++;

                                //le damos el formato que se necesita en la vista: Lun 01/01/23 9:59 am
                                var dayAbsence = checkOutTime.ToString("ddd dd/MM/yy hh:mm tt");

                                //le damos el formato que necesita la vista 10:10 am
                                var checkOutTimePuntual = schedulesCheckOut.ToString("hh: mm tt");
                                //se agrega a lista de faltas

                                employeeForIncidentReport.AbsenceDatesWithoutExit.Add($"{dayAbsence} -> {checkOutTimePuntual}");
                            }

                        }
                        else // si el numero de checadas es menor a 2 entonces es falta
                        {
                            //aumenta 1 al numero de faltas
                            employeeForIncidentReport.Absences++;

                            //le damos el formato que se necesita en la vista: Lun 01/01/23
                            var dayAbsence = Convert.ToDateTime(day).ToString("ddd dd/MM/yy"); ;

                            //se agrega a lista de faltas
                            employeeForIncidentReport.AbsenceDates.Add(dayAbsence);
                        }


                    }

                }

                //si las listas estan vacias les insertamos un registro vacio
                if (employeeForIncidentReport.Absences == 0)
                {
                    employeeForIncidentReport.AbsenceDates.Add(string.Empty);
                }
                if (employeeForIncidentReport.LongerDelays == 0)
                {
                    employeeForIncidentReport.LongerDelaysDates.Add(string.Empty);
                }
                if (employeeForIncidentReport.MinorDelays == 0)
                {
                    employeeForIncidentReport.MinorDelayDates.Add(string.Empty);
                }
                if (employeeForIncidentReport.AbsenceDatesWithoutExit.Count == 0)
                {
                    employeeForIncidentReport.AbsenceDatesWithoutExit.Add(string.Empty);
                }


                ////sumamos las faltas + los retardos menores + los retardos mayores
                //employeeForIncidentReport.TotalIncidents = employeeForIncidentReport.Absences + employeeForIncidentReport.MinorDelays + employeeForIncidentReport.LongerDelays;

                //se debe calcular el total de descuentos de la siguiente manera
                //cada 3 retardos menores es una falta
                //un retardo mayor es una falta
                var EveryThreeTardiesIsAnAbsence = 3;

                var absencesForDelayMinors = employeeForIncidentReport.MinorDelays / EveryThreeTardiesIsAnAbsence;

                employeeForIncidentReport.TotalIncidents = absencesForDelayMinors + employeeForIncidentReport.Absences
                    + employeeForIncidentReport.LongerDelays;

                ////sino hay almenos 1 incidencia en el rango de dias entonces el empleado no se agrega al reporte
                ////si hay un almenos 1 retardo tambien agregamos al empleado al reporte
                //if (employeeForIncidentReport.TotalIncidents > 0 || employeeForIncidentReport.MinorDelays > 0)
                //{
                //    response.Issuccessful = true;
                //    return response;
                //}

                response.Issuccessful = true;
                response.Payload = employeeForIncidentReport;


                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                response.Issuccessful = false;
                response.Errors = ErrorMessages.AddMessageError(Messages.InternalServerError, ex.Message);
                response.Status = 500;
            }

            return response;
        }


    }
}
