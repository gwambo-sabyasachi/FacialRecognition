using System;
using System.Collections.Generic;
using System.Text;

namespace FacialRecognition.Domain.DTOs
{
    public class TransactionItemDTO
    {
        public int id { get; set; }
        public string pin { get; set; }
        public string ename { get; set; }
        public string deptnumber { get; set; }
        public string deptname { get; set; }
        public string sn { get; set; }
        public string checktime { get; set; }
        public int verify { get; set; }
        public int stateno { get; set; }
        public string state { get; set; }

    }
}
