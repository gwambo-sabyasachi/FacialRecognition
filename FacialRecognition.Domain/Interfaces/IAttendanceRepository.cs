using FacialRecognition.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FacialRecognition.Domain.Interfaces
{
    public interface IAttendanceRepository
    {
        CptAttendance AttendanceExists(int pin, DateTime date);
        CptAttendance GetAttendanceOfLastDayWithoutAsync(int userId);
        public string InsertAttendance(CptAttendance attendance);
        public string UpdateAttendance(CptAttendance attendance);
        

    }
}
