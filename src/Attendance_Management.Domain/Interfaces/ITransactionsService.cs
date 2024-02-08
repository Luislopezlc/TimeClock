using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;

namespace Attendance_Management.Domain.Interfaces
{
    public interface ITransactionsService
    {
        Task<ResponseDto> GetIncidentReport(IncidentReportRequestDto request);
        Task<ResponseDto> AddIclock_Transaction(iclock_transactionDto request);
        Task<ResponseDto> AddIclock_Transactions(List<iclock_transactionDto> request);
        Task<ResponseDto> GetTransactionsByRange(List<string> employeesCodes, DateTime startDate, DateTime endDate);
        Task<ResponseDto> GetIclock_Transaction(int Id);
        Task<ResponseDto> GetEmployeesWithEmail();
        Task<ResponseDto> GetIncidentsIndividualReport(string employee, string initialDate, string endDate);
        Task<ResponseDto> GetPunchesOneDayForEmployeeCode(string date, string employeeCode);

        Task<ResponseDto> SendIncidentsByEmail(IncidenctsReportEmailDto request,ConfigurationSMPT configuration);
        Task<ResponseDto> GetIncidentsToday();
        Task<bool> sendEmail(IncidentsIndividualReportEmployeeDto incidentReportDtp, EmployeeDto employee, ConfigurationSMPT  configurationSMPT);
        Task<string> CustomBody(IncidentsIndividualReportEmployeeDto employeeIncidentReport, string pathEmail);
        string dateListTable(List<string> dates);
        
    }
}
