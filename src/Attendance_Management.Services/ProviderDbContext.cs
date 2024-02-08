using Attendance_Management.Domain;
using Attendance_Management.Domain.External;
using Attendance_Management.Domain.Interfaces;

namespace Attendance_Management.Services
{
	public class ProviderDbContext : IProviderDbContext
	{
		private readonly AppDbContext AppContext;
		private readonly ExternalDbContext ExternalDbContext;

		public ProviderDbContext(AppDbContext appContext, ExternalDbContext externalDbContext)
		{
			AppContext = appContext;
			ExternalDbContext = externalDbContext;
			
		}
		public AppDbContext GetAppDbContext()
		{
			return this.AppContext;
		}

        public ExternalDbContext GetExternalDbContext()
        {
			return this.ExternalDbContext;
        }
    }
}
