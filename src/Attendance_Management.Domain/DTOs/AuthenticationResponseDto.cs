namespace Attendance_Management.Domain.DTOs
{
	public class AuthenticationResponseDto
	{
		public string Token { get; set; }
		public DateTime Expiration { get; set; }
	}
}
