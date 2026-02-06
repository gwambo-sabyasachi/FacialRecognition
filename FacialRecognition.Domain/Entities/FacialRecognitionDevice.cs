using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FacialRecognition.Domain.Entities
{
    [Table("FacialRecognitionDevice")]
    public class FacialRecognitionDevice
    {
        public int Id { get; set; }
        public string? DeviceSN { get; set; }
        public bool? IsActive { get; set; }
        public string? Country {  get; set; }
        public string ? TimeZone { get; set; }
        public DateTime? CreatedAt { get; set; }


    }
}
