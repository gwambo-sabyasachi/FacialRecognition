using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FacialRecognition.Domain.Entities
{
    [Table("FacialRecognitionLog")]
    public class FacialRecognitionLog
    {
        public int Id { get; set; }
        public string Parameters { get; set; }
        public int ListCount { get; set; }
        public string ApiStatus { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DeviceSN { get; set; }
        public string CheckTime { get; set; }
        public string ErrorMessage { get; set; }
    }
}
