using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using IAuthorizationService = Attendance_Management.Domain.Interfaces.IAuthorizationService;

namespace Attendance_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        //payload: List<SidebarDto>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetSidebar()
        {

            var userName = HttpContext.User.Claims.Where(s => s.Type.Equals("UserName")).FirstOrDefault().Value;

            var response = await this.authorizationService.GetSidebar(userName);

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
        //payload: List<RolDto>
        [HttpGet]
        [Route("RolDto")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetRolDto()
        {
            var response = await this.authorizationService.GetRolDto();

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
        [Route("GetViewPermission")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetViewPermission([FromQuery] string viewName)
        {
            var userName = HttpContext.User.Claims.Where(s => s.Type.Equals("UserName")).FirstOrDefault().Value;

            var response = await this.authorizationService.CanYouSeeTheView(viewName, userName);

            if (response.Status == 400 && !response.Issuccessful)
            {
                return BadRequest(response);
            }
            else if (response.Status == 403 && !response.Issuccessful) 
            {
                return StatusCode(StatusCodes.Status403Forbidden, response);

            }
            else if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }


            [HttpGet]
        [Route("RolesBySearch")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetRolesBySearch([FromQuery] string request)
        {
            var response = await this.authorizationService.GetRolesBySearch(request);

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
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> AddRolesDto([FromBody] RolDto request)
        {
            var response = await this.authorizationService.AddRolDto(request);

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

        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> DeleteRol([FromRoute] string Id)
        {
            var response = await this.authorizationService.DeleteRol(Id);

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
