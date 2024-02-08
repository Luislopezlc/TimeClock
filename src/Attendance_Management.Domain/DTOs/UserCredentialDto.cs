using System.ComponentModel.DataAnnotations;

namespace Attendance_Management.Domain.DTOs
{
	public class UserCredentialDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
