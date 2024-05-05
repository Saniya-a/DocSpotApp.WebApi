using System;
using System.ComponentModel.DataAnnotations.Schema;
using static DocSpotApp.Models.Utility;

namespace DocSpotApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public Status ApprovalStatus { get; set; }
        public bool IsDeleted { get; set; }
    }
}

