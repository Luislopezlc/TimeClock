using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class UsersPaginatedListDto
    {
        public List<UserDto> Users { get; set; }
        public string Search { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
