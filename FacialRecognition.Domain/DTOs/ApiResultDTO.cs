using System;
using System.Collections.Generic;
using System.Text;

namespace FacialRecognition.Domain.DTOs
{
    public class ApiResultDTO
    {
        public bool IsSuccess { get; set; }
        public string Response { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}
