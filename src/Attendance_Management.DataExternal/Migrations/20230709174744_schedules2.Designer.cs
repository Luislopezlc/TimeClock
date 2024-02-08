﻿// <auto-generated />
using System;
using Attendance_Management.DataExternal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Attendance_Management.DataExternal.Migrations
{
    [DbContext(typeof(ExternalDbContext))]
    [Migration("20230709174744_schedules2")]
    partial class schedules2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Attendance_Management.DataExternal.DTOs.iclock_transactionToProcessDto", b =>
                {
                    b.Property<string>("employeCode")
                        .HasColumnType("text");

                    b.Property<int>("id")
                        .HasColumnType("integer");

                    b.Property<DateTime>("punchDate")
                        .HasColumnType("timestamp with time zone");

                    b.ToTable("iclock_TransactionToProcessDtos");
                });

            modelBuilder.Entity("Attendance_Management.DataExternal.Models.Att_Days", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("att_days");
                });

            modelBuilder.Entity("Attendance_Management.DataExternal.Models.Att_payloadbase", b =>
                {
                    b.Property<string>("uuid")
                        .HasColumnType("text");

                    b.Property<int>("_short")
                        .HasColumnType("integer")
                        .HasColumnName("short");

                    b.Property<int>("absent")
                        .HasColumnType("integer");

                    b.Property<int>("actual_worked")
                        .HasColumnType("integer");

                    b.Property<DateTime>("att_date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("break_time_id")
                        .HasColumnType("text");

                    b.Property<DateTime>("check_in")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("check_out")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("clock_in")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("clock_out")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("day_off")
                        .HasColumnType("integer");

                    b.Property<int>("duration")
                        .HasColumnType("integer");

                    b.Property<int>("duty_duration")
                        .HasColumnType("integer");

                    b.Property<int>("duty_worked")
                        .HasColumnType("integer");

                    b.Property<int>("early_leave")
                        .HasColumnType("integer");

                    b.Property<int>("emp_id")
                        .HasColumnType("integer");

                    b.Property<string>("exception")
                        .HasColumnType("text");

                    b.Property<int>("late")
                        .HasColumnType("integer");

                    b.Property<int>("leave")
                        .HasColumnType("integer");

                    b.Property<string>("overtime_id")
                        .HasColumnType("text");

                    b.Property<int>("remaining")
                        .HasColumnType("integer");

                    b.Property<int>("timetable_id")
                        .HasColumnType("integer");

                    b.Property<int>("total_time")
                        .HasColumnType("integer");

                    b.Property<int>("total_worked")
                        .HasColumnType("integer");

                    b.Property<int?>("trans_in_id")
                        .HasColumnType("integer");

                    b.Property<int?>("trans_out_id")
                        .HasColumnType("integer");

                    b.Property<int>("unscheduled")
                        .HasColumnType("integer");

                    b.Property<int>("weekday")
                        .HasColumnType("integer");

                    b.Property<double>("work_day")
                        .HasColumnType("double precision");

                    b.HasKey("uuid");

                    b.ToTable("att_payloadbase");
                });

            modelBuilder.Entity("Attendance_Management.DataExternal.Models.iclock_transaction", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("area_alias")
                        .HasColumnType("text");

                    b.Property<string>("crc")
                        .HasColumnType("text");

                    b.Property<string>("emp_code")
                        .HasColumnType("text");

                    b.Property<int?>("emp_id")
                        .HasColumnType("integer");

                    b.Property<string>("gps_location")
                        .HasColumnType("text");

                    b.Property<int?>("is_attendance")
                        .HasColumnType("integer");

                    b.Property<int?>("is_mask")
                        .HasColumnType("integer");

                    b.Property<decimal?>("latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("longitude")
                        .HasColumnType("numeric");

                    b.Property<string>("mobile")
                        .HasColumnType("text");

                    b.Property<string>("punch_state")
                        .HasColumnType("text");

                    b.Property<DateTime>("punch_time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("purpose")
                        .HasColumnType("integer");

                    b.Property<int?>("reserved")
                        .HasColumnType("integer");

                    b.Property<int?>("source")
                        .HasColumnType("integer");

                    b.Property<int?>("sync_status")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("sync_time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("temperature")
                        .HasColumnType("integer");

                    b.Property<string>("terminal_alias")
                        .HasColumnType("text");

                    b.Property<int?>("terminal_id")
                        .HasColumnType("integer");

                    b.Property<string>("terminal_sn")
                        .HasColumnType("text");

                    b.Property<DateTime?>("upload_time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("verify_type")
                        .HasColumnType("integer");

                    b.Property<string>("work_code")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("iclock_transaction");
                });

            modelBuilder.Entity("Attendance_Management.DataExternal.Models.Personnel_department", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("company_id")
                        .HasColumnType("integer");

                    b.Property<string>("dept_code")
                        .HasColumnType("text");

                    b.Property<string>("dept_name")
                        .HasColumnType("text");

                    b.Property<bool>("is_default")
                        .HasColumnType("boolean");

                    b.Property<int?>("parent_dept_id")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("personnel_department");
                });

            modelBuilder.Entity("Attendance_Management.DataExternal.Models.Personnel_employee", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("acc_group")
                        .HasColumnType("text");

                    b.Property<string>("acc_timezone")
                        .HasColumnType("text");

                    b.Property<string>("address")
                        .HasColumnType("text");

                    b.Property<int?>("app_role")
                        .HasColumnType("integer");

                    b.Property<int?>("app_status")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("birthday")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("card_no")
                        .HasColumnType("text");

                    b.Property<DateTime?>("change_time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("change_user")
                        .HasColumnType("text");

                    b.Property<string>("city")
                        .HasColumnType("text");

                    b.Property<int?>("company_id")
                        .HasColumnType("integer");

                    b.Property<string>("contact_tel")
                        .HasColumnType("text");

                    b.Property<DateTime?>("create_time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("create_user")
                        .HasColumnType("text");

                    b.Property<int?>("del_tag")
                        .HasColumnType("integer");

                    b.Property<bool>("deleted")
                        .HasColumnType("boolean");

                    b.Property<int?>("department_id")
                        .HasColumnType("integer");

                    b.Property<int?>("dev_privilege")
                        .HasColumnType("integer");

                    b.Property<string>("device_password")
                        .HasColumnType("text");

                    b.Property<string>("driver_license_automobile")
                        .HasColumnType("text");

                    b.Property<string>("driver_license_motorcycle")
                        .HasColumnType("text");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<int>("emp_code")
                        .HasColumnType("integer");

                    b.Property<int?>("emp_type")
                        .HasColumnType("integer");

                    b.Property<bool>("enable_att")
                        .HasColumnType("boolean");

                    b.Property<bool>("enable_holiday")
                        .HasColumnType("boolean");

                    b.Property<bool>("enable_overtime")
                        .HasColumnType("boolean");

                    b.Property<bool>("enable_payroll")
                        .HasColumnType("boolean");

                    b.Property<string>("enroll_sn")
                        .HasColumnType("text");

                    b.Property<string>("first_name")
                        .HasColumnType("text");

                    b.Property<string>("gender")
                        .HasColumnType("text");

                    b.Property<DateTime?>("hire_date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("internal_emp_num")
                        .HasColumnType("text");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<bool>("is_admin")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("last_login")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("last_name")
                        .HasColumnType("text");

                    b.Property<string>("mobile")
                        .HasColumnType("text");

                    b.Property<string>("national")
                        .HasColumnType("text");

                    b.Property<string>("national_num")
                        .HasColumnType("text");

                    b.Property<string>("nickname")
                        .HasColumnType("text");

                    b.Property<string>("office_tel")
                        .HasColumnType("text");

                    b.Property<string>("passport")
                        .HasColumnType("text");

                    b.Property<string>("payroll_num")
                        .HasColumnType("text");

                    b.Property<string>("photo")
                        .HasColumnType("text");

                    b.Property<int?>("position_id")
                        .HasColumnType("integer");

                    b.Property<string>("postcode")
                        .HasColumnType("text");

                    b.Property<string>("religion")
                        .HasColumnType("text");

                    b.Property<int?>("reserved")
                        .HasColumnType("integer");

                    b.Property<string>("self_password")
                        .HasColumnType("text");

                    b.Property<string>("ssn")
                        .HasColumnType("text");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.Property<string>("title")
                        .HasColumnType("text");

                    b.Property<DateTime?>("update_time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("vacation_rule")
                        .HasColumnType("integer");

                    b.Property<int?>("verify_mode")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("personnel_employee");
                });

            modelBuilder.Entity("Attendance_Management.DataExternal.Models.Personnel_position", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("company_id")
                        .HasColumnType("integer");

                    b.Property<bool>("is_default")
                        .HasColumnType("boolean");

                    b.Property<int?>("parent_position_id")
                        .HasColumnType("integer");

                    b.Property<string>("position_code")
                        .HasColumnType("text");

                    b.Property<string>("position_name")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("personnel_position");
                });

            modelBuilder.Entity("Attendance_Management.DataExternal.Models.Personnel_Schedules", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CheckInTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CheckOutTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DayId")
                        .HasColumnType("integer");

                    b.Property<string>("EmployeeCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DayId");

                    b.ToTable("personnel_schedules");
                });

            modelBuilder.Entity("Attendance_Management.DataExternal.Models.Personnel_Schedules", b =>
                {
                    b.HasOne("Attendance_Management.DataExternal.Models.Att_Days", "Day")
                        .WithMany("PersonnelSchedules")
                        .HasForeignKey("DayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Day");
                });

            modelBuilder.Entity("Attendance_Management.DataExternal.Models.Att_Days", b =>
                {
                    b.Navigation("PersonnelSchedules");
                });
#pragma warning restore 612, 618
        }
    }
}
