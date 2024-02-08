using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.Models
{
    public class AreasUser
    {
        [Key]
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string UserId { get; set; }
        public bool IsLeader { get; set; }
        public int PositionId { get; set; }
        [ForeignKey("PositionId")]
        public Position Position { get; set; }
        [ForeignKey("AreaId")]
        public Area Area { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }
    }


}
