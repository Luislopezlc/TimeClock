using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.DataExternal.DTOs
{
	[Keyless]
	public class iclock_transactionToProcessDto
	{
		public int id { get; set; }
		public string employeCode { get; set; }
		public DateTime punchDate { get; set; }
	}
}
