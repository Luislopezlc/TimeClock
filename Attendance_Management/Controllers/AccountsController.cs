
using Attendance_Management.Domain;
using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Attendance_Management.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Attendance_Management.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly UserManager<AppUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IConfiguration configuration;
		private readonly SignInManager<AppUser> signInManager;
		private readonly AppDbContext appDbContext;
		private readonly IEmployeeService employeeService;
		private readonly IUsersService usersService;
		private readonly IBusinessStructureService businessStructureService;

		public AccountsController(UserManager<AppUser> userManager, AppDbContext appDbContext,
			IConfiguration configuration, SignInManager<AppUser> signInManager, IEmployeeService employeeService, IUsersService usersService,
			IBusinessStructureService businessStructureService, RoleManager<IdentityRole> roleManager)
		{
			this.businessStructureService= businessStructureService;
			this.usersService = usersService;	
			this.appDbContext= appDbContext;
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.configuration = configuration;
			this.signInManager = signInManager;
			this.employeeService = employeeService;
		}

		[HttpPost]
		[Route("LogInUser")]
		[SwaggerResponse((int)HttpStatusCode.OK)]
		[SwaggerResponse((int)HttpStatusCode.NotFound)]

		public async Task<ActionResult<AuthenticationResponseDto>> LoginUser(UserCredentialDto request)
		{
			var response = await this.signInManager.PasswordSignInAsync(request.Email, request.Password
				, isPersistent: false, lockoutOnFailure: false);

			if (response.Succeeded)
			{
				return CreateToken(request);
			}

			var messageError = "Correo o contraseña incorrectos";

			var responseError = new ResponseDto();
			var listErros = new List<string>();
			listErros.Add(messageError);
			responseError.Errors.Add("User", listErros);	

			return NotFound(responseError);

		}

        [HttpPost]
        [Route("LogInGoogle")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        public async Task<ActionResult<AuthenticationResponseDto>> LogInGoogle(UserCredentialsGoogleDto request)
        {
			const string rolName = "Empleado";

			var response = new ResponseDto();

			var userCredentials = new UserCredentialDto();

			var firstError = new IdentityError();

			var messageError = string.Empty;

            var user = new AppUser()
            {
                FirstName = request.GivenName,
                LastName = request.Surname,
				Email = request.Email,
				UserName = request.Email,
            };

			var userExist = await this.usersService.GetUser(user.Email);

			if(userExist.Status == 500)
            {
				return BadRequest(userExist);
            }

			if (!userExist.Issuccessful)
            {
				var userEmpCode = await this.employeeService.GetEmpCodeByName(request.GivenName, request.Surname);

				if(userEmpCode.Payload == null)
                {
					response.Payload = null;
					response.Issuccessful = false;
					response.Status = 400;
					response.Errors = ErrorMessages.AddMessageError("EmpCodeNotFound", "No se  encontro al empleado.");
					return BadRequest(response);
                }

				user.EmployeeCode = userEmpCode.Payload.ToString();

				var result = await this.userManager.CreateAsync(user);

                if (result.Succeeded)
                {
					var searchRol = await this.roleManager.FindByNameAsync(rolName);
					var searchUser = await this.userManager.FindByEmailAsync(request.Email);

					if(searchRol != null)
                    {
						var createNewUserRol = await this.userManager.AddToRoleAsync(searchUser, searchRol.Name);

						if (createNewUserRol.Succeeded)
						{
							userCredentials.Email = user.Email;
							var userToken = CreateToken(userCredentials);

							response.Payload = userToken;
							response.Issuccessful = true;

							return Ok(response);
						}

						firstError = createNewUserRol.Errors.FirstOrDefault();
						messageError = firstError != null ? firstError.Description : "Error al crear el usuario";

						response.Payload = null;
						response.Errors = ErrorMessages.AddMessageError("Error rol", messageError);
						response.Status = 400;
                    }
                    else
                    {
						response.Payload = null;
						response.Errors = ErrorMessages.AddMessageError("Error rol", "El rol que se intenta asignar no existe");
						response.Status = 400;
                    }
				}

				firstError = result.Errors.FirstOrDefault();
					
				messageError = firstError != null ? firstError.Description : "Error al crear el usuario";

				response.Payload = null;
				response.Errors = ErrorMessages.AddMessageError("Error Usuario", messageError);
				response.Status = 400;

				return BadRequest(response);
            }
            else
            {
				userCredentials.Email = request.Email;
				var userToken = CreateToken(userCredentials);

				response.Payload = userToken;
				response.Issuccessful = true;

				return Ok(response);
			}
        }


        [HttpGet]
		[Route("RenewToken")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[SwaggerResponse((int)HttpStatusCode.OK)]
		public ActionResult<AuthenticationResponseDto> RenewToken()
		{
			var claimEmail = HttpContext.User.Claims.Where(c => c.Type == "UserName").FirstOrDefault();
			var email = claimEmail.Value;
			var userCredentials = new UserCredentialDto()
			{
				Email = email,
			};

			return Ok(CreateToken(userCredentials));


		}

        [HttpPost]
        [Route("RegisterUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<AuthenticationResponseDto>> RegisterUser(RegisterUserDto request)
        {
            var userCredentials = new UserCredentialDto();
            userCredentials.Email = request.Email;
            userCredentials.Password = request.Password;

            var user = new AppUser()
            {
                UserName = request.Email,
                Email = request.Email,
            };

            var response = await userManager.CreateAsync(user, request.Password);

            var userResponse = this.appDbContext.AppUsers.FirstOrDefault(u => u.Email.Equals(request.Email));


            if (userResponse != null)
            {
                userResponse.FirstName = request.FirstName;
                userResponse.LastName = request.LastName;
                userResponse.CreationDate = DateTime.Now.ToUniversalTime();
                userResponse.EmployeeCode = request.EmployeeCode;
                userResponse.IsActive = true;
                this.appDbContext.Attach(userResponse);
                this.appDbContext.Entry(userResponse).State = EntityState.Modified;
                this.appDbContext.SaveChanges();
            }

            if (response.Succeeded)
            {
                return CreateToken(userCredentials);
            }
            else
            {
                var errors = GetErrosInSpanish();

                return NotFound(errors.Where(e => e.Key.Equals(response.Errors.FirstOrDefault().Code)).FirstOrDefault().Value);
            }
        }

       

        private AuthenticationResponseDto CreateToken(UserCredentialDto request)
		{
			var claims = new List<Claim>()
			{
				new Claim("UserName", request.Email)
			};
			var key = this.configuration["KeyJwt"];

            var keyJwt = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			var credencials = new SigningCredentials(keyJwt, SecurityAlgorithms.HmacSha256);

			var expiration = DateTime.Now.AddMinutes(20);

			var securityToken = new JwtSecurityToken(
				issuer: "Api_TimeClock_issuer",
				audience: "Api_TimeClock",
				claims: claims,
				expires: expiration,
				signingCredentials: credencials
			
				);

			var response = new AuthenticationResponseDto()
			{
				Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
				Expiration = expiration,
			};

			return response;
		}
		//Tipo de dato y nombre del metodo
		private Dictionary<string, string> GetErrosInSpanish()
		{
			var errors = new Dictionary<string, string>();

			errors.Add("PasswordRequiresNonAlphanumeric", "Las contraseñas deben tener al menos un carácter no alfanumérico.");
			errors.Add("PasswordRequiresDigit", "Las contraseñas deben tener al menos un dígito ('0'-'9').");
			errors.Add("PasswordRequiresUpper", "Las contraseñas deben tener al menos una letra mayúscula ('A'-'Z').");
			errors.Add("DuplicateUserName", "Este correo ya esta en uso.");
			errors.Add("PasswordRequiresLower", "Las contraseñas deben tener al menos una minúscula ('a'-'z').");

			return errors;
		}
	}
}
