using FacialRecognition.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacialRecognition.Infrastructure.Data
{
    public class FacialRecognitionDbContext:DbContext
    {
        public FacialRecognitionDbContext(DbContextOptions<FacialRecognitionDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<FacialRecognitionLog> facialRecognitionLog { get; set; }
        public DbSet<FacialRecognitionDevice> facialRecognitionDevices { get; set; }
        public DbSet<CptAttendance> cptAttendances { get; set; }
        public DbSet<CPT_UserShiftAllocationCal> cpt_UserShiftAllocationCals { get; set; }
    }
}
