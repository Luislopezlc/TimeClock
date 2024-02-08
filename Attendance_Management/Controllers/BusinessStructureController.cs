using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Attendance_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class BusinessStructureController : ControllerBase
    {
        private readonly IBusinessStructureService businessStructureService;

        public BusinessStructureController(IBusinessStructureService businessStructureService)
        {
            this.businessStructureService = businessStructureService;
        }

        [HttpGet]
        [Route("GetDepartmentCodes")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetDepartmentCodes([FromQuery]string search)
        {
            var response = await this.businessStructureService.GetDepartmentsCode(search);
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

        [HttpGet]
        [Route("GetDepartments")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetDepartments()
        {
            var response = await this.businessStructureService.GetDeparments();
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

        [HttpGet]
        [Route("GetDepartmentDto/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetDepartmentDto([FromRoute] int id)
        {
            var response = await this.businessStructureService.GetDeparmentsDto(id);
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

        [HttpDelete]
        [Route("DeleteDepartment/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> DeleteDepartment([FromRoute] int id)
        {
            var response = await this.businessStructureService.DeleteDepartment(id);
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

        [HttpGet]
        [Route("GetAreaDto/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetAreaDto([FromRoute] int id)
        {
            var response = await this.businessStructureService.GetAreaDto(id);
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

        [HttpGet]
        [Route("GetDepartmentsName")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetDepartmentsName([FromQuery] string Search)
        {
            var response = await this.businessStructureService.GetDeparmentsName(Search);
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

        [HttpGet]
        [Route("GetAreas")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetAreas()
        {
            var response = await this.businessStructureService.GetAreas();
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

        [HttpGet]
        [Route("GetAreasName")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetAreasName([FromQuery] string Search)
        {
            var response = await this.businessStructureService.GetAreasName(Search);
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


        [HttpGet]
        [Route("GetArea/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetArea([FromRoute] int id)
        {
            var response = await this.businessStructureService.GetArea(id);

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

        [HttpDelete]
        [Route("DeleteArea/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> DeleteArea([FromRoute] int id)
        {
            var response = await this.businessStructureService.DeleteArea(id);

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


        [HttpGet]
        [Route("GetPositions")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetPositions()
        {
            var response = await this.businessStructureService.GetPositions();
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

        [HttpGet]
        [Route("GetPosition/{Id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetPosition([FromRoute] int Id)
        {
            var response = await this.businessStructureService.GetPositionById(Id);
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

        [HttpDelete]
        [Route("DeletePosition/{Id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> DeletePosition([FromRoute] int Id)
        {
            var response = await this.businessStructureService.DeletePosition(Id);
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
        [Route("PostDepartment")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> PostDepartment([FromBody] DeparmentDto request)
        {
            var response = await this.businessStructureService.PostDeparments(request);
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
        [Route("PostArea")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> PostArea([FromBody] AreaDto request)
        {
            var response = await this.businessStructureService.PostArea(request);
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
        [Route("PostPosition")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> PostPosition([FromBody] PositionDto request)
        {
            var response = await this.businessStructureService.PostPosition(request);
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

        [HttpGet]
        [Route("GetPositionsPaginated")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetPositionsPaginated([FromQuery] PositionListPaginatedDto request)
        {
            var response = await this.businessStructureService.GetPositionListPaginatedDto(request);
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

        [HttpGet]
        [Route("GetDeparmentsPaginated")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetDeparmentsPaginated([FromQuery] DeparmentListPaginatedDto request)
        {
            var response = await this.businessStructureService.GetDeparmentsListPaginatedDto(request);
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

        [HttpGet]
        [Route("GetAreasPaginated")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetAreasPaginated([FromQuery] AreaListPaginatedDto request)
        {
            var response = await this.businessStructureService.GetAreaListPaginatedDto(request);
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

        [HttpGet]
        [Route("GetPositionsBySearch")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetPositionsBySearch([FromQuery] string Search)
        {
            var response = await this.businessStructureService.GetPositionsBySearch(Search);
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
