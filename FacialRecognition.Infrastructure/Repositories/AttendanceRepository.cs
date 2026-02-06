using FacialRecognition.Common;
using FacialRecognition.Domain.Entities;
using FacialRecognition.Domain.Interfaces;
using FacialRecognition.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FacialRecognition.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly FacialRecognitionDbContext _context;
        public AttendanceRepository(FacialRecognitionDbContext context)
        {
            _context = context;
        }
        public CptAttendance AttendanceExists(int pin, DateTime date)
        {
            try
            {
                CptAttendance attendance = new CptAttendance();
                List<SqlParameter> lstParam = new List<SqlParameter>();
                lstParam.Add(new SqlParameter("@UserId", pin));
                lstParam.Add(new SqlParameter("@AttendanceDate", date));
                DataTable dt = FacialRecognitionCommonSql.ExecuteStoredProcedure("SP_ChkAttendanceExistsForApi", lstParam);
                if(dt != null && dt.Rows.Count> 0)
                {
                    attendance.Id = dt.Rows[0]["Id"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Id"]):0;
                    attendance.UserId = dt.Rows[0]["UserId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["UserId"]):0;
                    attendance.AttendanceDate = dt.Rows[0]["AttendanceDate"]!= DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["AttendanceDate"]) : DateTime.MinValue;
                    attendance.PunchIn = dt.Rows[0]["PunchIn"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["PunchIn"]) : DateTime.MinValue;
                    attendance.PunchOut = dt.Rows[0]["PunchOut"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["PunchOut"]) : DateTime.MinValue;
                    return attendance;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }   

        }
        public CptAttendance GetAttendanceOfLastDayWithoutAsync(int userId)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@UserId", userId)
                };
                DataTable dt = FacialRecognitionCommonSql.ExecuteStoredProcedure("sp_GetAttendanceOfLastDay", parameters); 
                if (dt == null || dt.Rows.Count == 0)
                    return null;
                DataRow row = dt.Rows[0];
                return new CptAttendance
                {
                    Id = row["Id"] != DBNull.Value? Convert.ToInt32(row["Id"]): 0,
                    UserId = row["UserId"] != DBNull.Value? Convert.ToInt32(row["UserId"]): userId,
                    AttendanceDate = row["AttendanceDate"] != DBNull.Value? Convert.ToDateTime(row["AttendanceDate"]): DateTime.MinValue,
                    PunchIn = row["PunchIn"] != DBNull.Value? Convert.ToDateTime(row["PunchIn"]): null,
                    PunchOut = row["PunchOut"] != DBNull.Value? Convert.ToDateTime(row["PunchOut"]): null
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string InsertAttendance(CptAttendance attendance)
        {
            try
            {
                List<SqlParameter> lstParam = new List<SqlParameter>();
                lstParam.Add(new SqlParameter("@UserId", attendance.UserId));
                lstParam.Add(new SqlParameter("@AttendanceDate", attendance.AttendanceDate));
                lstParam.Add(new SqlParameter("@PunchIn", attendance.PunchIn));
                DataTable dt = FacialRecognitionCommonSql.ExecuteStoredProcedure("SP_InsertAttandanceDetails", lstParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Message"].ToString();
                }
                return "No record updated";
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public string UpdateAttendance(CptAttendance attendance)
        {
            try
            {
                List<SqlParameter> lstParam = new List<SqlParameter>();
                lstParam.Add(new SqlParameter("@UserId", attendance.UserId));
                lstParam.Add(new SqlParameter("@AttendanceDate", attendance.AttendanceDate));
                lstParam.Add(new SqlParameter("@PunchOut", attendance.PunchOut));
                DataTable dt = FacialRecognitionCommonSql.ExecuteStoredProcedure("SP_UpdatePunchOut", lstParam);
                if(dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Message"].ToString();
                }
                return "No record updated";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
