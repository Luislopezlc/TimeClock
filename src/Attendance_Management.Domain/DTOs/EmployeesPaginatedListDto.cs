using Attendance_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class EmployeesPaginatedListDto
    {
        public List<EmployeeDto> Employees { get; set; } = new();

        public PaginationDto Pagination { get; set; }
    }
}
