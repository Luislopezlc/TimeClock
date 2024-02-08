namespace Attendance_Management.Domain.DTOs
{
    public class EmployeeIncidentReportDto
    {
        public int Id { get; set; } //se usa el employeeCode
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName
        {
            get
            {
                return $"{Id} - {FirstName} {LastName}";
            }
            set
            {
                FullName = value;
            }
        }
        public string Job { get; set; }
        public int Absences { get; set; }
        public List<string> AbsenceDates { get; set; } = new();
        public List<string> AbsenceDatesWithoutExit { get; set; } = new();
        public int MinorDelays { get; set; }
        public List<string> MinorDelayDates { get; set; } = new();
        public int LongerDelays { get; set; }
        public List<string> LongerDelaysDates { get; set; } = new();
        public int TotalIncidents { get; set; }
    }
}
