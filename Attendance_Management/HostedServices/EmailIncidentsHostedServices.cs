using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Attendance_Management
{
    public class EmailIncidentsHostedServices : IHostedService, IDisposable
    {
        private readonly IConfiguration configuration;
        private readonly IServiceProvider serviceProvider; //se utiliza para inyectar servicios 
        private readonly ITransactionsService transactionsService;
        private readonly IGeneralConfigurationService generalConfigurationService;
        private readonly ILogger<EmailIncidentsHostedServices> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private Timer timer;

        public EmailIncidentsHostedServices(IConfiguration configuration, ILogger<EmailIncidentsHostedServices> logger, IServiceProvider serviceProvider
            , IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
            this.serviceProvider = serviceProvider;
            var scoped = this.serviceProvider.CreateScope();
            this.transactionsService = scoped.ServiceProvider.GetService<ITransactionsService>();
            this.logger = logger;
            this.generalConfigurationService = scoped.ServiceProvider.GetService<IGeneralConfigurationService>();

        }
        public void Dispose()
        {
            this.timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Timed Hosted Service running.");

            this.timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(24));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                var configurationSMTP = new ConfigurationSMPT();
                this.configuration.GetSection("ConfigurationSMPT").Bind(configurationSMTP);
                //obtenemos la fecha del dia actual en formato utc
                var wwwrootPath = webHostEnvironment.WebRootPath;
                var filePath = Path.Combine(wwwrootPath, "IncidentReportEmail.html");

                if (File.Exists(filePath))
                {
                    configurationSMTP.PathEmail = filePath;
                }

                var isGeneralConfigurationActited = await this.generalConfigurationService.GetGeneralConfiguration("AutomaticEmails");
                bool automaticEmailsActived = false;
                if (isGeneralConfigurationActited.Issuccessful)
                {
                    if (isGeneralConfigurationActited.Payload is GeneralConfiguration config)
                    {
                        automaticEmailsActived = (bool)config.BoolValue;
                    }
                }
                else
                {
                    throw new Exception("Error in AutomaticEmails Configuration");
                }

                if (configurationSMTP.Activated && automaticEmailsActived)
                {
                    var response = await this.transactionsService.SendIncidentsByEmail(null, configurationSMTP);

                    if (!response.Issuccessful)
                    {
                        logger.LogError(response.Errors.Values.FirstOrDefault().FirstOrDefault());
                    }
                }
            }
            catch (Exception e)
            {
                //se cancela el DoWork?
                Console.WriteLine(e.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Timed Hosted Service is stopping.");

            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
