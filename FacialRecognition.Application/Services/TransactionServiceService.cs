using FacialRecognition.Domain.DTOs;
using FacialRecognition.Domain.Entities;
using FacialRecognition.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace FacialRecognition.Application.Services
{
    public class TransactionServiceService
    {
        public readonly ITransactionService _repository;
        public readonly IFacialRecognition _facialrepository;
        public readonly IFacialRecognitionDevice _facialRecognitionDevice;
        public readonly IAttendanceRepository _attendanceRepository;
        public TransactionServiceService(ITransactionService repository,IFacialRecognition facialrepository, IFacialRecognitionDevice facialRecognitionDevice, IAttendanceRepository attendanceRepository)
        {
            _repository = repository;
            _facialrepository = facialrepository;
            _facialRecognitionDevice = facialRecognitionDevice;
            _attendanceRepository = attendanceRepository;
        }
        //public async Task RunTransactionCronAsync()
        //{
        //    var devices = _facialRecognitionDevice.getAllDeviceList();
        //    if (devices == null || devices.Count == 0)
        //        return;



        //    foreach (var device in devices)
        //    {
        //        if (string.IsNullOrEmpty(device.DeviceSN))
        //            continue;
        //        var lastLog = _facialrepository.GetFacialRecognitionLogByDeviceSN(device.DeviceSN);

        //        //DateTime startTime = lastLog?.EndTime ?? DateTime.UtcNow.AddMinutes(-5);
        //        DateTime startTime;
        //        if (lastLog == null)
        //        {
        //            startTime = DateTime.UtcNow.AddDays(-1);
        //            startTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(startTime, device.TimeZone);
        //        }
        //        else
        //        {
        //            startTime = lastLog.EndTime.GetValueOrDefault();
        //        }
        //        DateTime endTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow,device.TimeZone);
        //        if (lastLog != null && endTime <= lastLog.EndTime)
        //            continue;
        //        if (startTime >= endTime)
        //            continue;
        //        await GetTransactionsAsync(startTime, endTime, device);
        //    }
        //}

        public async Task<string> RunTransactionCronAsync()
        {
            try
            {
                var devices = _facialRecognitionDevice.getAllDeviceList();
                if (devices == null || devices.Count == 0)
                    return "No Device Found";
                foreach (var device in devices)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(device.DeviceSN))
                            continue;

                        var lastLog = _facialrepository.GetFacialRecognitionLogByDeviceSN(device.DeviceSN);

                        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(device.TimeZone);
                        var utcNow = DateTime.UtcNow;

                        DateTime startTime = lastLog == null
                            ? TimeZoneInfo.ConvertTimeFromUtc(utcNow.AddDays(-1), timeZone)
                            : lastLog.EndTime.GetValueOrDefault();

                        DateTime endTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, device.TimeZone);

                        if (lastLog != null && endTime <= lastLog.EndTime)
                            continue;

                        if (startTime >= endTime)
                            continue;

                        var message = await GetTransactionsAsync(startTime, endTime, device);

                        if (!string.Equals(message, "Success", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(message, "No data found", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                return "Transaction cron executed successfully";
            }
            catch (Exception ex)
            {
                return $"Error in RunTransactionCronAsync: {ex.Message}";
            }
        }


        //public async Task<int> GetTransactionsAsync(DateTime startDate,DateTime endDate,FacialRecognitionDevice device)
        //{
        //    if (device == null)
        //        return 0;

        //    var apiResponse = await _repository.GetTransactionsAsync(startDate,endDate,device.DeviceSN);
        //    if (!apiResponse.IsSuccess)
        //    {
        //        _facialrepository.CreateFacialRecognitionLog(
        //            new FacialRecognitionLog
        //            {
        //                Parameters = $"startTime={startDate}, endTime={endDate}, sn={device.DeviceSN}",
        //                ListCount = 0,
        //                ApiStatus = "failed",
        //                StartTime = startDate,
        //                EndTime = endDate,
        //                DeviceSN = device.DeviceSN,
        //                CheckTime = Convert.ToString(endDate),
        //                ErrorMessage = apiResponse.ErrorMessage
        //            });
        //        return 0;
        //    }
        //    var result = DeserializeResponse(apiResponse.Response);

        //    if (result == null ||result.data == null ||result.data.count <= 0 ||result.data.items == null || result.data.items.Count == 0)
        //        return 0;

        //    var status = InsertUpdateAttendanceBl(result.data.items);

        //   if (status == "success")
        //    {
        //        _facialrepository.CreateFacialRecognitionLog(
        //            new FacialRecognitionLog
        //            {
        //                Parameters = $"startTime={startDate}, endTime={endDate}, sn={device.DeviceSN}",
        //                ListCount = result.data.count,
        //                ApiStatus = "success",
        //                StartTime = startDate,
        //                EndTime = endDate,
        //                DeviceSN = device.DeviceSN,
        //                CheckTime = Convert.ToString(endDate)
        //            });
        //    }
        //    return result.data.count;
        //}

        public async Task<string> GetTransactionsAsync(DateTime startDate, DateTime endDate, FacialRecognitionDevice device)
        {

            try
            {
                if (device == null)
                    return "Device is null";

                var apiResponse = await _repository.GetTransactionsAsync(startDate, endDate, device.DeviceSN);

                if (!apiResponse.IsSuccess)
                {
                    _facialrepository.CreateFacialRecognitionLog(
                        new FacialRecognitionLog
                        {
                            Parameters = $"startTime={startDate}, endTime={endDate}, sn={device.DeviceSN}",
                            ListCount = 0,
                            ApiStatus = "failed",
                            StartTime = startDate,
                            EndTime = endDate,
                            DeviceSN = device.DeviceSN,
                            CheckTime = endDate.ToString(),
                            ErrorMessage = apiResponse.ErrorMessage
                        });

                    return $"Failed: {apiResponse.ErrorMessage}";
                }

                var result = DeserializeResponse(apiResponse.Response);

                if (result == null || result.data == null || result.data.count <= 0 ||result.data.items == null || result.data.items.Count == 0)
                {
                    return "No data found";
                }

                var status = InsertUpdateAttendanceBl(result.data.items);

                if (status == "success")
                {
                    _facialrepository.CreateFacialRecognitionLog(
                        new FacialRecognitionLog
                        {
                            Parameters = $"startTime={startDate}, endTime={endDate}, sn={device.DeviceSN}",
                            ListCount = result.data.count,
                            ApiStatus = "success",
                            StartTime = startDate,
                            EndTime = endDate,
                            DeviceSN = device.DeviceSN,
                            CheckTime = endDate.ToString()
                        });

                    return "Success";
                }

                return "Data processing failed";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private TransactionApiResponseDTO DeserializeResponse(string apiResponse)
        {
            return JsonSerializer.Deserialize<TransactionApiResponseDTO>(apiResponse,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public string InsertUpdateAttendanceBl(List<TransactionItemDTO> items)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            var groupedData = items
                .GroupBy(x => x.pin)
                .Select(g => new
                {
                    UserId = Convert.ToInt32(g.Key),
                    Records = g.OrderBy(x => x.checktime).ToList()
                }).ToList();

            foreach (var employee in groupedData)
            {
                foreach (var record in employee.Records)
                {
                    DateTime punchTime = Convert.ToDateTime(record.checktime);

                    if (_attendanceRepository.IsDuplicatePunch(employee.UserId, punchTime))
                    {
                        continue;
                    }  
                    var attendanceExists = _attendanceRepository.AttendanceExists(employee.UserId, punchTime);

                    if (attendanceExists == null)
                    {
                        var lastDayAttendance =_attendanceRepository.GetAttendanceOfLastDayWithoutAsync(employee.UserId);
                        if (lastDayAttendance != null && lastDayAttendance.PunchOut == null)
                        {
                            var shift =_repository.GetUserShiftAllocationCal(employee.UserId,lastDayAttendance.AttendanceDate);
                            if (shift != null && shift.OpenEndedShift == true)
                            {
                                //  Punch the last daus
                                lastDayAttendance.PunchOut = punchTime;
                                _attendanceRepository.UpdateAttendance(lastDayAttendance);
                                continue;
                            }
                        }

                        //  New Punch IN on PunchTimedate
                        _attendanceRepository.InsertAttendance(new CptAttendance
                        {
                            UserId = employee.UserId,
                            AttendanceDate = punchTime.Date,
                            PunchIn = punchTime
                        });
                    }
                    else if (attendanceExists.PunchIn != null && (attendanceExists.PunchOut == null || attendanceExists.PunchOut == DateTime.MinValue))
                    {
                        // Punch OUT on that day day
                        attendanceExists.PunchOut = punchTime;
                        _attendanceRepository.UpdateAttendance(attendanceExists);
                    }
                    else
                    {
                        //  PunchIn & PunchOut exist  New Punch IN
                        _attendanceRepository.InsertAttendance(new CptAttendance
                        {
                            UserId = employee.UserId,
                            AttendanceDate = punchTime.Date,
                            PunchIn = punchTime
                        });
                    }
                }
            }
            return "success";
        }



    }
}
