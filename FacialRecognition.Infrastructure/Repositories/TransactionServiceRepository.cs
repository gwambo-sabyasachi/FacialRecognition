using Azure.Core;
using FacialRecognition.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FacialRecognition.Infrastructure.Repositories
{
    public class TransactionServiceServiceRepository : ITransactionService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public TransactionServiceServiceRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
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
        
    }
}
