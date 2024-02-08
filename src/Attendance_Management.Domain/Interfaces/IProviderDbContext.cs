using Attendance_Management.Domain.External;

namespace Attendance_Management.Domain.Interfaces
{
	public interface IProviderDbContext
	{
		AppDbContext GetAppDbContext();
		ExternalDbContext GetExternalDbContext();
	}
}
