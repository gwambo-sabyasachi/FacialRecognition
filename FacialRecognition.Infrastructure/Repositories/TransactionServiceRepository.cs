using Azure.Core;
using FacialRecognition.Common;
using FacialRecognition.Domain.DTOs;
using FacialRecognition.Domain.Entities;
using FacialRecognition.Domain.Interfaces;
using FacialRecognition.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;

namespace FacialRecognition.Infrastructure.Repositories
{
    public class TransactionServiceServiceRepository : ITransactionService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly FacialRecognitionDbContext _context;
        public TransactionServiceServiceRepository(HttpClient httpClient, IConfiguration configuration, FacialRecognitionDbContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> GetTransactionsAsync()
        {
            var baseUrl = _configuration["ExternalApis:TransactionApiUrl"];
            var apiKey = _configuration["ExternalApis:TransactionApiKey"];
            var url = $"{baseUrl}?key={apiKey}";
            var json = JsonSerializer.Serialize(new
            {
                starttime = "2026-01-29 00:00:00",
                endtime = "2026-01-29 23:59:59",
                sn = "CJ9G232360463"
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<ApiResultDTO> GetTransactionsAsync(DateTime startTime,DateTime endTime,string deviceSn)
        {
            try
            {
                var baseUrl = _configuration["ExternalApis:TransactionApiUrl"];
                var apiKey = _configuration["ExternalApis:TransactionApiKey"];
                var url = $"{baseUrl}?key={apiKey}";
                var payload = new
                {
                  starttime = startTime.ToString("yyyy-MM-dd HH:mm:ss"),
                  endtime = endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                  sn = deviceSn
                };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResultDTO
                    {
                        IsSuccess = false,
                        StatusCode = (int)response.StatusCode,
                        ErrorMessage = responseBody
                    };
                }
                return new ApiResultDTO
                {
                    IsSuccess = true,
                    StatusCode = (int)response.StatusCode,
                    Response = responseBody
                };
            }
            catch (Exception ex)
            {
                return new ApiResultDTO
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }


        public CPT_UserShiftAllocationCal GetUserShiftAllocationCal(int userId, DateTime shiftDate)
        {
            CPT_UserShiftAllocationCal cal = new CPT_UserShiftAllocationCal();
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(new SqlParameter("@UserId", userId));
            lstParam.Add(new SqlParameter("@ShiftDate", shiftDate));
            DataTable dt = FacialRecognitionCommonSql.ExecuteStoredProcedure("SP_GetUserShiftAllocationCalForApi", lstParam);
            if (dt == null || dt.Rows.Count == 0)
                return null;
            if (dt != null && dt.Rows.Count > 0)
            {
                cal.Id = dt.Rows[0]["Id"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Id"]) : string.Empty;
                cal.DepartmentId = dt.Rows[0]["DepartmentId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["DepartmentId"]) : 0;
                cal.UserId = dt.Rows[0]["UserId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["UserId"]) : 0;
                cal.ShiftDate = dt.Rows[0]["ShiftDate"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["ShiftDate"]) : DateTime.MinValue;
                cal.ShiftStartTime = dt.Rows[0]["ShiftStartTime"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["ShiftStartTime"]) : DateTime.MinValue;
                cal.ShiftEndTime = dt.Rows[0]["ShiftEndTime"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["ShiftEndTime"]) : DateTime.MinValue;
                cal.OvertimeStartTime = dt.Rows[0]["OvertimeStartTime"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["OvertimeStartTime"]) : DateTime.MinValue;
                cal.OvertimeEndTime = dt.Rows[0]["OvertimeEndTime"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["OvertimeEndTime"]) : DateTime.MinValue;
                cal.AddedBy = dt.Rows[0]["AddedBy"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["AddedBy"]) : 0;
                cal.OpenEndedShift = dt.Rows[0]["OpenEndedShift"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["OpenEndedShift"]) : false;
                cal.ShiftNameId = dt.Rows[0]["ShiftNameId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["ShiftNameId"]) : 0;
                cal.WorkingDays = dt.Rows[0]["WorkingDays"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["WorkingDays"]) : false;
                cal.BankHolidays = dt.Rows[0]["BankHolidays"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["BankHolidays"]) : false;
                cal.Weekend = dt.Rows[0]["Weekend"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["Weekend"]) : false;
            }
            return cal;

        }
    }
}
