namespace Attendance_Management.Domain.DTOs
{
    public class ListAttendanceDto
    {
        public string InitialDate { get; set; }
        public string EndDate { get; set; }
        public int Days { get; set; }
        public List<EmployeeAttendaceListDto> Employees { get; set; } = new();
        
    }
}
