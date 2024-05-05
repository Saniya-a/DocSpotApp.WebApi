using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DocSpotApp.Models.Utility;

namespace DocSpotApp.ViewModels
{
    public class AppointmentVM
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }

        public string AppointmentTime { get; set; }
        public Status ApprovalStatus { get; set; }
        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }
        public string? HospitalName { get; set;}
        public int? HospitalId { get; set; }
        public string? DepartmentName { get; set; }
        public int? DepartmentId { get; set; }


    }
}


