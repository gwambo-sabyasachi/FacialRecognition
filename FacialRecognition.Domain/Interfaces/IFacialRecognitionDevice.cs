using FacialRecognition.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacialRecognition.Domain.Interfaces
{
    public interface IFacialRecognitionDevice
    {
        List<FacialRecognitionDevice> getAllDeviceList();
        FacialRecognitionDevice GetDetailsByDeviceSN(string deviceSN);
        FacialRecognitionDevice GetDetailsById(int id);
    }
}
