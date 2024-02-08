using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
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
    public class HolidaysController : Controller
    {
        public IHolidaysService HolidaysService;

        public HolidaysController(IHolidaysService holidaysService)
        {
            HolidaysService = holidaysService;
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetHolidays([FromQuery] bool? isActive)
        {
            var response = await this.HolidaysService.GetHolidays(isActive);
            if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if (response.Status == 200 && response == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("{Id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetHoliday([FromRoute] int Id)
        {
            var response = await this.HolidaysService.GetHoliday(Id);
            if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if (response.Status == 200 && response == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetHolidayDto/{Id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetHolidayDto([FromRoute] int Id)
        {
            var response = await this.HolidaysService.GetHolidayDto(Id);
            if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if (response.Status == 200 && response == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteHoliday/{Id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> DeleteHoliday([FromRoute] int Id)
        {
            var response = await this.HolidaysService.DeleteHoliday(Id);
            if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if (response.Status == 200 && response == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("ByDay/{day}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetHoliday(string day)
        {
            var response = await this.HolidaysService.GetHoliday(day);
            if(response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }else if(response.Status == 200 && response == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("GetHolidays")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetListPaginatedHolidays([FromQuery] HolidaysPaginatedListDto request)
        {
            var response = await this.HolidaysService.GetListPaginatedHolidays(request);
            if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if (response.Status == 200 && response == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> AddHolidays([FromBody] AddHolidayDto request)
        {
            var response = await this.HolidaysService.AddHolidays(request);
            if(response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else if(response.Status == 200 && !response.Issuccessful)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
