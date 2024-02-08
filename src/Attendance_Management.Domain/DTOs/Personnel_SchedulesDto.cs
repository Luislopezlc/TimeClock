using System.ComponentModel.DataAnnotations;

namespace Attendance_Management.Domain.DTOs
{
    public class Personnel_SchedulesDto
    {
        [Required(ErrorMessage = "El campo codigo de empleado es un campo requerido")]
        public string EmployeeCode { get; set; }
        [Required(ErrorMessage = "El campo hora de entrada es un campo requerido")]
        public string CheckInTime { get; set; }
        [Required(ErrorMessage = "El campo hora de salida es un campo requerido"),]
        public string CheckOutTime { get; set; }
        [Required(ErrorMessage = "El campo dia es un campo requerido"),]
        public int ValueDay { get; set; }
    }
}
