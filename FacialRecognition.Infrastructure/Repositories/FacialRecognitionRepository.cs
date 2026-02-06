using FacialRecognition.Common;
using FacialRecognition.Domain.DTOs;
using FacialRecognition.Domain.Entities;
using FacialRecognition.Domain.Interfaces;
using FacialRecognition.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FacialRecognition.Infrastructure.Repositories
{
    public class FacialRecognitionRepository : IFacialRecognition
    {
        private readonly FacialRecognitionDbContext _context;
        public FacialRecognitionRepository(FacialRecognitionDbContext context)
        {
            _context = context;
        }
        public FacialRecognitionLog GetStartDateFromLastTransaction()
        {
             var data=_context.facialRecognitionLog.Where(f => f.ApiStatus == "success").OrderByDescending(f => f.Id).FirstOrDefault();
            return data;     
        }
        public FacialRecognitionLog GetFacialRecognitionLogByDeviceSN(string deviceSN)
        {
            try
            {
                List<SqlParameter> lstParam = new List<SqlParameter>();
                lstParam.Add(new SqlParameter("@DeviceSN", deviceSN));
                DataTable dt = FacialRecognitionCommonSql.ExecuteStoredProcedure("SP_GetFacialRecognitionLogByDeviceSN", lstParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    FacialRecognitionLog log = new FacialRecognitionLog
                    {
                        Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                        Parameters = dt.Rows[0]["Parameters"].ToString(),
                        ListCount = dt.Rows[0]["ListCount"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["ListCount"]) : 0,
                        ApiStatus = dt.Rows[0]["ApiStatus"].ToString(),
                        StartTime = dt.Rows[0]["StartTime"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["StartTime"]) : (DateTime?)null,
                        EndTime = dt.Rows[0]["EndTime"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["EndTime"]) : (DateTime?)null,
                        CreatedAt = dt.Rows[0]["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["CreatedAt"]) : (DateTime?)null,
                        DeviceSN = dt.Rows[0]["DeviceSN"].ToString(),
                        CheckTime = dt.Rows[0]["CheckTime"].ToString(),
                        ErrorMessage = dt.Rows[0]["ErrorMessage"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["ErrorMessage"]).ToString(): string.Empty,
                    };
                    return log;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public string CreateFacialRecognitionLog(FacialRecognitionLog log)
        {
            DataTable dt = new DataTable();
            try
            {
                List<SqlParameter> lstParam = new List<SqlParameter>();
                lstParam.Add(new SqlParameter("@Parameters", log.Parameters));
                lstParam.Add(new SqlParameter("@ListCount", log.ListCount));
                lstParam.Add(new SqlParameter("@ApiStatus", log.ApiStatus));
                lstParam.Add(new SqlParameter("@StartTime", log.StartTime));
                lstParam.Add(new SqlParameter("@EndTime", log.EndTime));
                lstParam.Add(new SqlParameter("@DeviceSN", log.DeviceSN));
                lstParam.Add(new SqlParameter("@CheckTime", log.CheckTime));
                lstParam.Add(new SqlParameter("@ErrorMessage", log.ErrorMessage));
                dt = FacialRecognitionCommonSql.ExecuteStoredProcedure("SP_CreateFacialRecognitionLog", lstParam);
                if(dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Result"].ToString();
                }
                return "FAILED";
            }
            catch (Exception ex)
            {
                return "";
            }
           
        }
    }
}
