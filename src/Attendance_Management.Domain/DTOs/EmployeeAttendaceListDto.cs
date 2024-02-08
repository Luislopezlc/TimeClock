namespace Attendance_Management.Domain.DTOs
{
    public class EmployeeAttendaceListDto
    {
        public string FullName { get; set; }
        public string Job { get; set; }
        public List<AttendaceDetailDto> Days { get; set; } = new();

    }
}
