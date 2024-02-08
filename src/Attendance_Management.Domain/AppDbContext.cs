using Attendance_Management.Domain.DTOs;
using Attendance_Management.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Attendance_Management.Domain
{
	public class AppDbContext : IdentityDbContext<AppUser>
    {
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		
        public DbSet<Att_Days> att_days { get; set; }
        public DbSet<Personnel_Schedules> personnel_schedules { get; set; }
		public DbSet<Auth_Sidebar> auth_sidebar { get; set; }
		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<Area> areas { get; set; }
		public DbSet<Position> positions { get; set; }
		public DbSet<Deparment> deparments { get; set; }
		public DbSet<AreasUser> areasUsers { get; set; }
		public DbSet<Att_Holidays> att_holidays { get; set; }
		public DbSet<Att_HolidaysEmployees> att_holidaysEmployees { get; set; }
		public DbSet<sidebarRoles> sidebarRoles { get; set; }
		public DbSet<GeneralConfiguration> GeneralConfiguration { get; set; }
	

	protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
