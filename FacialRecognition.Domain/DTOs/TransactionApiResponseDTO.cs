using System;
using System.Collections.Generic;
using System.Text;

namespace FacialRecognition.Domain.DTOs
{
    public class TransactionApiResponseDTO
    {
        public int ret { get; set; }
        public string msg { get; set; }
        public TransactionData data { get; set; }
    }

    public class TransactionData
    {
        public int count { get; set; }
        public List<TransactionItemDTO> items { get; set; }
    }
}
