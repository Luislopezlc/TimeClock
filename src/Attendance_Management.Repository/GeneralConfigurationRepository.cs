using Attendance_Management.Domain;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Repository
{
    public class GeneralConfigurationRepository : IGeneralConfigurationRepository
    {
        private readonly IProviderDbContext providerDbContext;
        private readonly AppDbContext dbContext;
        public GeneralConfigurationRepository(IProviderDbContext providerDbContext)
        {
             this.providerDbContext = providerDbContext;
            this.dbContext = this.providerDbContext.GetAppDbContext();
        }
        public async Task<bool> ExistsGeneralConfiguration(string name)
        {
            var generalConfiguration = await this.dbContext.GeneralConfiguration.FirstOrDefaultAsync(x => x.Name == name);
            return generalConfiguration != null;
        }

        public async Task<GeneralConfiguration> GetGeneralConfiguration(string ConfigurationName)
        {
            var generalConfiguration = await this.dbContext.GeneralConfiguration.FirstOrDefaultAsync(x => x.Name == ConfigurationName);
            return generalConfiguration;
        }

        public async Task<GeneralConfiguration> UpdateGeneralConfiguration(GeneralConfiguration request)
        {
            var generalConfiguration = await this.GetGeneralConfiguration(request.Name);

            generalConfiguration.BoolValue = request.BoolValue;
            generalConfiguration.StringValue = request.StringValue;
            generalConfiguration.DoubleValue = request.DoubleValue;
            generalConfiguration.IntValue = request.IntValue;

            this.dbContext.Attach(generalConfiguration);
            this.dbContext.Entry(generalConfiguration).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();

            return generalConfiguration;
        }
    }
}
