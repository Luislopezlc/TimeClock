using Attendance_Management.Domain;
using Attendance_Management.Domain.Interfaces;
using Attendance_Management.Domain.Models;
using Attendance_Management.Repository;
using Attendance_Management.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using EntityFrameworkCore.UseRowNumberForPaging;
using Microsoft.OpenApi.Models;
using System.Text;
using Attendance_Management.Domain.External;
using Attendance_Management;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var Configuration = builder.Configuration;
var connectionStringExternal = builder.Configuration
							  .GetConnectionString("ExternalString") ??
							  throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<ExternalDbContext>(options => options
											.UseNpgsql(connectionStringExternal));

var connectionString = builder.Configuration
                              .GetConnectionString("DefaultString") ??
                              throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<AppDbContext>(options => options
                                            .UseSqlServer(connectionString, builder => builder.UseRowNumberForPaging()));



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    

})
    .AddJwtBearer(options =>
    {
        options.Audience = "Api_TimeClock";
        options.TokenValidationParameters = new TokenValidationParameters
        {
        ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "Api_TimeClock_issuer",
            ValidAudience = "Api_TimeClock",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["KeyJwt"])),
            ClockSkew = TimeSpan.Zero,

        };
    });



builder.Services.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders",
          builder =>
          {
              builder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
          });
});

builder.Services.AddScoped<IProviderDbContext, ProviderDbContext>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
builder.Services.AddScoped<ITransactionsService, TransactionsService>();
builder.Services.AddScoped<ISchedulesRepository, SchedulesRepository>();
builder.Services.AddScoped<ISchedulesService, SchedulesService>();
builder.Services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersService, UsersServices>();
builder.Services.AddScoped<IBusinessStructureRepository, BusinessStructureRepository>();
builder.Services.AddScoped<IBusinessStructureService, BusinessStructureService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<IHolidaysRepository, HolidaysRepository>();
builder.Services.AddScoped<IHolidaysService, HolidaysService>();
builder.Services.AddScoped<IGeneralConfigurationRepository, GeneralConfigurationRepository>();
builder.Services.AddScoped<IGeneralConfigurationService, GeneralConfigurationService>();
builder.Services.AddHostedService<EmailIncidentsHostedServices>();



var app = builder.Build();


app.UseCors("AllowAllHeaders");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
