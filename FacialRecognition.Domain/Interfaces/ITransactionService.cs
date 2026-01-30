using System;
using System.Collections.Generic;
using System.Text;

namespace FacialRecognition.Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<string> GetTransactionsAsync();
    }
}
