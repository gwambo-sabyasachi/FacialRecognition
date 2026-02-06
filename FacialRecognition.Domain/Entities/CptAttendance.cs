using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FacialRecognition.Domain.Entities
{
    [Table("CPT_Attendance")]
    public class CptAttendance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime? PunchIn { get; set; }
        public DateTime? PunchOut { get; set; }
        public DateTime? BreakIn { get; set; }
        public DateTime? BreakOut { get; set; }
        public bool IsOvertimeApprove { get; set; }
        public int? ManagerId { get; set; }
        public decimal? OverTime { get; set; }
        public string OvertimeComments { get; set; }
        public string InIP { get; set; }
        public string InLat { get; set; }
        public string InLong { get; set; }
        public string OutIp { get; set; }
        public string OutLat { get; set; }
        public string OutLong { get; set; }
        public bool? LockAttendance { get; set; }
        public bool? IsWFH { get; set; }
        public int? WFHApprovedBy { get; set; }

    }
}
