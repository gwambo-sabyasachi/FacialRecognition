using FacialRecognition.Domain.DTOs;
using FacialRecognition.Domain.Entities;
using FacialRecognition.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace FacialRecognition.Application.Services
{
    public class FacialRecognitionService
    {
        public readonly IFacialRecognition _repository;
        public FacialRecognitionService(IFacialRecognition repository)
        {
            _repository = repository;
        }

        public FacialRecognisionDTO? GetStartDateFromLastTransaction()
        {
            FacialRecognisionDTO facialRecognisionDTO = new FacialRecognisionDTO();
           var h = _repository.GetStartDateFromLastTransaction();

            facialRecognisionDTO.Id=h.Id;
                return facialRecognisionDTO;
        }
    }
}
