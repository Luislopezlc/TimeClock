using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Attendance_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class TransactionsController : ControllerBase
    {

        private readonly ITransactionsService transactionsService;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IGeneralConfigurationService generalConfigurationService;
        public TransactionsController(ITransactionsService transactionsService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IGeneralConfigurationService generalConfigurationService)
        {
            this.transactionsService = transactionsService;
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
            this.generalConfigurationService = generalConfigurationService;
        }
        //payload: iclock_transaction
        [HttpGet("{Id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetIclock_Transaction([FromRoute] int Id)
        {
            var response = await this.transactionsService.GetIclock_Transaction(Id);
            if (response.Status == 400 && !response.Issuccessful)
            {
                return BadRequest(response);
            }
            else if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if (response.Issuccessful && response.Payload == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        //payload: IncidentsIndividualReportEmployeeDto
        [HttpPost()]
        [Route("GetIncidentsIndividualReport")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetIncidentsIndividualReport([FromBody] IncidentsIndividualReportDto request)
        {
            var response = await this.transactionsService.GetIncidentsIndividualReport(request.EmployeeCode, request.InitialDate, request.EndDate);

            if (response.Status == 400 && !response.Issuccessful)
            {
                return BadRequest(response);
            }
            else if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if (response.Issuccessful && response.Payload == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        //payload: List<IncidentToday>
        [HttpGet()]
        [Route("IncidentsToday")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetIncidentsToday()
        {
            var response = await this.transactionsService.GetIncidentsToday();

            if (response.Status == 400 && !response.Issuccessful)
            {
                return BadRequest(response);
            }
            else if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if (response.Issuccessful && response.Payload == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("PostGeneralConfiguration")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> PostGeneralConfiguration([FromBody]GeneralConfiguration request)
        {
            var response = await this.generalConfigurationService.UpdateGeneralConfiguration(request);

            if (response.Status == 400 && !response.Issuccessful)
            {
                return BadRequest(response);
            }
            else if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if (response.Issuccessful && response.Payload == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("GetGeneralConfiguration")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> GetGeneralConfiguration([FromQuery] string ConfigurationName)
        {
            var response = await this.generalConfigurationService.GetGeneralConfiguration(ConfigurationName);

            if (response.Status == 400 && !response.Issuccessful)
            {
                return BadRequest(response);
            }
            else if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if (response.Issuccessful && response.Payload == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("SendEmails")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult> SendEmails([FromBody] SendEmailsDto request)
        {
            await Console.Out.WriteLineAsync($"Entro al endpoint de los correos {DateTime.Now}");

            if (!request.ApiKey.Equals(this.configuration["ApiKey"]))
            {
                return NotFound();
            }

            var response = new ResponseDto();

            try
            {
                var configurationSMTP = new ConfigurationSMPT();
                this.configuration.GetSection("ConfigurationSMPT").Bind(configurationSMTP);

                var wwwrootPath = webHostEnvironment.WebRootPath;
                var filePath = Path.Combine(wwwrootPath, "IncidentReportEmail.html");

                if (System.IO.File.Exists(filePath))
                {
                    configurationSMTP.PathEmail = filePath;
                }

                response = await this.transactionsService.SendIncidentsByEmail(request?.IncidenctsReportEmailDto, configurationSMTP);
                if (response.Status == 400 && !response.Issuccessful)
                {
                    return BadRequest(response);
                }
                else if (response.Status == 500 && !response.Issuccessful)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(response);
            }

            return Ok(response);
        }
        //payload: List<iclock_transactionToProcessDto>
        [HttpPost]
        [Route("GetTransactionsByRange")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetTransactionsByRange([FromBody]TransactionsByRangeDto request)
        {

            var response = await this.transactionsService.GetTransactionsByRange(request.EmployeeCodes, Convert.ToDateTime(request.InitialDate).ToUniversalTime(), Convert.ToDateTime(request.EndDate).ToUniversalTime());
            if (response.Status == 400 && !response.Issuccessful)
            {
                return BadRequest(response);
            }
            else if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }
        

        [HttpPost]
        [Route("GenerateIncidentRepor")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetIncidentRepor(IncidentReportRequestDto request)
        {
            var response = await this.transactionsService.GetIncidentReport(request);

            if (response.Status == 400 && !response.Issuccessful)
            {
                return BadRequest(response);
            }
            else if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }
    }
}
