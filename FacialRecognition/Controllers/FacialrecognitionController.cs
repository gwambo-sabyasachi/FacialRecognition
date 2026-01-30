using FacialRecognition.Application.Services;
using FacialRecognition.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace FacialRecognition.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacialrecognitionController : ControllerBase
    {
        private readonly FacialRecognitionService _service;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly TransactionServiceService _transactionService;
        public FacialrecognitionController(FacialRecognitionService service, TransactionServiceService transactionService)
        {
            _service = service;
            _transactionService = transactionService;
        }

        [HttpGet]
        [Route("[action]")]
        [Route("api/Facialrecognition/GetLatestSuccessRecord")]
        [AllowAnonymous]

        public IActionResult GetLatestSuccessRecord() {
            var latestData = _service.GetStartDateFromLastTransaction(); 
            return Ok(latestData);
        }

        [HttpGet]
        [Route("[action]")]
        [Route("api/Facialrecognition/GetLatestAttendanceRecord")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestAttendanceRecord() {
            var result = await _transactionService.GetTransactionsAsync();
            return Ok(result);

        }
    }
}
