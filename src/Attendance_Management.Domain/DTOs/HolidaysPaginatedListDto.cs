using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class HolidaysPaginatedListDto
    {
        public List<GetHolidaysDto> Holidays { get; set; }
        public PaginationDto Pagination { get; set; }
        public string Search { get; set; }
    }
}
