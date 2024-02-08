using Attendance_Management.Domain.External.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.External
{
    public class ExternalDbContext : DbContext
    {
        public ExternalDbContext(DbContextOptions<ExternalDbContext> options) : base(options)
        {

        }
        public DbSet<Personnel_employee> personnel_employee { get; set; }
        public DbSet<Personnel_department> personnel_department { get; set; }
        public DbSet<Personnel_position> personnel_position { get; set; }
        public DbSet<Att_payloadbase> att_payloadbase { get; set; }
        public DbSet<iclock_transaction> iclock_transaction { get; set; }
        public DbSet<iclock_transactionToProcessDto> iclock_TransactionToProcessDtos { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}
