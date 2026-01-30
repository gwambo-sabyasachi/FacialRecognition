using FacialRecognition.Domain.DTOs;
using FacialRecognition.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacialRecognition.Application.Services
{
    public class TransactionServiceService
    {
        public readonly ITransactionService _repository;
        public TransactionServiceService(ITransactionService repository)
        {
            _repository = repository;
        }

        public Task<string> GetTransactionsAsync()
        {  
            return _repository.GetTransactionsAsync(); 
        }
        
    }
}
