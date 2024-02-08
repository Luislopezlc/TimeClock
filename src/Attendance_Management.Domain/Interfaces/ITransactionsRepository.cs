using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.External.Models;
using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<List<iclock_transactionToProcessDto>> GetTransactionsByRange(List<string> employeesCodes,DateTime startDate, DateTime endDate);
        Task<List<iclock_transactionToProcessDto>> GetPunchesOneDayForEmployeeCode(string date, string employeeCode);

      //  Task<IncidentReportDto> GetIncidentReport(string initialDate,string EndDate);
        Task<iclock_transaction> GetIclock_Transaction(int Id);
        Task<iclock_transaction> AddIclock_Transaction(iclock_transactionDto request);
        Task<List<EmployeeDto>> GetEmployeesWithEmail();
        List<EmployeeDto> FakeEmployees();
            
    }
}
