using DocSpotApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocSpotApp.Repository.DAL.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<List<AppointmentVM>> GetAll();
        Task<List<AppointmentVM>> GetAppointmentsByPatientId(string patientId);
        Task<List<AppointmentVM>> GetAppointmentsByDoctorId(string doctorId);
        Task Add(AppointmentVM appointment);
        Task Update(AppointmentVM appointment);
        Task<AppointmentVM> GetById(int Id);
        Task ApproveAppointment(int appointmentId);
        Task RejectAppointment(int appointmentId);
    }
}
