using FacialRecognition.Domain.DTOs;
using FacialRecognition.Domain.Entities;
using FacialRecognition.Domain.Interfaces;
using FacialRecognition.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace FacialRecognition.Infrastructure.Repositories
{
    public class FacialRecognitionRepository : IFacialRecognition
    {
        private readonly FacialRecognitionDbContext _context;
        public FacialRecognitionRepository(FacialRecognitionDbContext context)
        {
            _context = context;
        }
        public FacialRecognitionLog GetStartDateFromLastTransaction()
        {
             var data=_context.facialRecognitionLog.Where(f => f.ApiStatus == "Success").OrderByDescending(f => f.Id).FirstOrDefault();
            return data;
               
        }
    }
}
