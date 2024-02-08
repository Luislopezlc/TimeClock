using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Attendance_Management.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeService employeeService;
		public EmployeeController(IEmployeeService employeeService)
		{
			this.employeeService = employeeService;
		}

        //Devuelve una lista de empleados paginada
        //payload: EmployeesPaginatedListDto
        [HttpGet]
		[Route("PaginatedList")]
		[SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetEmployeesPaginatedList([FromQuery] EmployeesPaginatedListWithFiltres request)
		{
			var response = await this.employeeService.EmployeesPaginatedList(request);
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

        //se utiliza en la barra de busqueda de la pantalla de empleados
        //payload: List<string>
        [HttpGet]
        [Route("GetEmployeeSearchBar/{search}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetEmployeeSearchBar([FromRoute] string search)
        {
            var response = await this.employeeService.GetEmployeeSearchBar(search);
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

        //se utiliza en el dashboard le paso los datos a la card de empleados por llegar
        //payload: List<EmployeeDto>
        [HttpGet]
        [Route("GetUpcomingEmployees")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetUpcomingEmployees()
        {
			var response = await this.employeeService.GetUpcomingEmployees();
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

        //payload: Personnel_employee
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> AddEmployee([FromBody] EmployeeDto request)
        {
            var response = await this.employeeService.AddEmployee(request);
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
