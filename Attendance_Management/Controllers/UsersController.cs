using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Security.Claims;

namespace Attendance_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        //payload: UserDto
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetUser()
        {

            var claim = HttpContext.User.Claims.Where(s => s.Type.Equals("UserName")).FirstOrDefault().Value;
            if (string.IsNullOrEmpty(claim))
            {
                return Ok(new UserDto());
            }

            var response = await this.usersService.GetUser(claim);
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
        [Route("GetUsersPaginatedList")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetUsersPaginatedList([FromQuery] UsersPaginatedListDto request)
        {


            var response = await this.usersService.GetUsersPaginatedList(request);
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
        [Route("GetUserById")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetUserById(string Id)
        {
            var response = await this.usersService.GetUserById(Id);
            if(response.Status == 400 && !response.Issuccessful)
            {
                return NotFound(response);
            }
            else if(response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetUserByEmpCode")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetUserByEmpCode(string empCode)
        {
            var response = await this.usersService.GetUserDto(empCode);
            if(response.Status == 400 && !response.Issuccessful)
            {
                return NotFound(response);
            }
            else if(response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetUsersBySearch")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResponseDto>> GetUsersBySearch([FromQuery]string search)
        {
            var response = await this.usersService.GetUsersBySearch(search);
            if (response.Status == 400 && !response.Issuccessful)
            {
                return NotFound(response);
            }
            else if (response.Status == 500 && !response.Issuccessful)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ResponseDto>> UpdaterUser(UpdateUserDto request)
        {
            var response = await this.usersService.UpdateUser(request);
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
        [Route("DeleteAreaUser/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ResponseDto>> UpdaterUser([FromRoute] int id)
        {
            var response = await this.usersService.DeleteAreasUser(id);
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
