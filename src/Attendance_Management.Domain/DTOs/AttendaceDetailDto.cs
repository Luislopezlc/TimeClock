namespace Attendance_Management.Domain.DTOs
{
    public class AttendaceDetailDto
    {
        //public int WorkingHours { get; set; }
        public string Date { get; set; }
        public bool RequiresAssistance { get; set; }
        public string CodeStatusForDay { get; set; }
    }
}
