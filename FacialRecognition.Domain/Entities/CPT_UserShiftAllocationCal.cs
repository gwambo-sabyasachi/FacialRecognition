using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FacialRecognition.Domain.Entities
{
    [Table("CPT_UserShiftAllocationCal")]
    public class CPT_UserShiftAllocationCal
    {
        public string Id { get; set; }
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
        public DateTime ShiftDate { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
        public DateTime OvertimeStartTime { get; set; }
        public DateTime OvertimeEndTime { get; set; }
        public int AddedBy { get; set; }
        public bool? OpenEndedShift { get; set; }
        public int? ShiftNameId { get; set; }
        public bool? WorkingDays { get; set; }
        public bool? BankHolidays { get; set; }
        public bool? Weekend { get; set; }
        public int? AnotherShiftId { get; set; }
    }
}
