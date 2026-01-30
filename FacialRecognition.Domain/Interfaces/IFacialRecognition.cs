using FacialRecognition.Domain.DTOs;
using FacialRecognition.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacialRecognition.Domain.Interfaces
{
    public interface IFacialRecognition
    {
        FacialRecognitionLog GetStartDateFromLastTransaction();
    }
}
