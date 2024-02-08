namespace Attendance_Management.Domain.DTOs
{
    public class IncidentReportDto
    {
        public string InitialDate { get; set; }
        public string EndDate { get; set; }
        public List<EmployeeIncidentReportDto> Employees { get; set; } = new();
        public PaginationDto Pagination { get; set; }
    }
}
