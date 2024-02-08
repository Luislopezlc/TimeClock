namespace Attendance_Management.Domain.DTOs
{
    public class TransactionsByRangeDto
    {
        public List<string> EmployeeCodes { get; set; }
        public string InitialDate { get; set; }
        public string EndDate { get; set; }
    }
}
