using FacialRecognition.Domain.DTOs;
using FacialRecognition.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacialRecognition.Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<string> GetTransactionsAsync();
        Task<ApiResultDTO> GetTransactionsAsync(DateTime startTime, DateTime endTime, string deviceSn);
        CPT_UserShiftAllocationCal GetUserShiftAllocationCal(int userId, DateTime shiftDate);

    }
}
