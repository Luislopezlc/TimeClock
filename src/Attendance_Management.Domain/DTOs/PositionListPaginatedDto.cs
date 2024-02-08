using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class PositionListPaginatedDto
    {
        public PaginationDto Pagination { get; set; } = new();
        public List<PositionDto> Positions { get; set; } = new();
        public string Search { get; set; }
    }
}
