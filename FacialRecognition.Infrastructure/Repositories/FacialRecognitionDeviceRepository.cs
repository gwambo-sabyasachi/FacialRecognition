using FacialRecognition.Domain.Entities;
using FacialRecognition.Domain.Interfaces;
using FacialRecognition.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacialRecognition.Infrastructure.Repositories
{
    public class FacialRecognitionDeviceRepository: IFacialRecognitionDevice
    {
        private readonly FacialRecognitionDbContext _context;

        public FacialRecognitionDeviceRepository(FacialRecognitionDbContext context)
        {
            _context = context;
        }

        public List<FacialRecognitionDevice> getAllDeviceList()
        {
            return _context.facialRecognitionDevices.Where(x => x.IsActive == true).ToList();
        }

        public FacialRecognitionDevice GetDetailsByDeviceSN(string deviceSN)
        {
            return _context.facialRecognitionDevices.Where(x => x.DeviceSN == deviceSN && x.IsActive == true).FirstOrDefault();
        }

        public FacialRecognitionDevice GetDetailsById(int id)
        {
             return _context.facialRecognitionDevices.Where(x => x.Id == id && x.IsActive == true).FirstOrDefault();
        }
    }
}
