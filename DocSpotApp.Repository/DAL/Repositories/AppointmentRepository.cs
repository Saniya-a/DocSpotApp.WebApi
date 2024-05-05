using DocSpotApp.Models;
using DocSpotApp.Repository.DAL.Interfaces;
using DocSpotApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocSpotApp.Repository.DAL.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DocSpotDBContext _dbContext;
        public AppointmentRepository(DocSpotDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<List<AppointmentVM>> GetAppointmentsByPatientId(string patientId)
        {
            try
            {
                var query = _dbContext.Appointments.Where(x => x.PatientId == patientId);
                return await query.Select(x => new AppointmentVM
                {
                    Id = x.Id,
                    DoctorId = x.DoctorId,
                    DoctorName = x.Doctor.FirstName + " " + x.Doctor.LastName,
                    DepartmentName = x.Doctor.Department.Name ?? "General Practicial",
                    HospitalName = x.Doctor.Hospital.Name,
                    DepartmentId = x.Doctor.DepartmentId,
                    HospitalId = x.Doctor.HospitalId,
                    AppointmentDate = x.AppointmentDate,
                    AppointmentTime = x.AppointmentTime,
                    ApprovalStatus = x.ApprovalStatus,

                }).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AppointmentVM>> GetAll()
        {
            try
            {

                return await _dbContext.Appointments.Select(x => new AppointmentVM
                {
                    Id = x.Id,
                    DoctorId = x.DoctorId,
                    PatientId = x.PatientId,
                    PatientName = x.Patient.FirstName + " " + x.Patient.LastName,
                    DoctorName = x.Doctor.FirstName + " " + x.Doctor.LastName,
                    DepartmentName = x.Doctor.Department.Name ?? "General Practicial",
                    HospitalName = x.Doctor.Hospital.Name,
                    DepartmentId = x.Doctor.DepartmentId,
                    HospitalId = x.Doctor.HospitalId,
                    AppointmentDate = x.AppointmentDate,
                    AppointmentTime = x.AppointmentTime,
                    ApprovalStatus = x.ApprovalStatus,

                }).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AppointmentVM>> GetAppointmentsByDoctorId(string doctorId)
        {
            try
            {
                var query = _dbContext.Appointments.Where(x => x.DoctorId == doctorId);
                return await query.Select(x => new AppointmentVM
                {
                    Id = x.Id,
                    PatientId = x.PatientId,
                    PatientName = x.Patient.FirstName + " " + x.Patient.LastName,
                    DepartmentName = x.Doctor.Department.Name ?? "General Practicial",
                    HospitalName = x.Doctor.Hospital.Name,
                    DepartmentId = x.Doctor.DepartmentId,
                    HospitalId = x.Doctor.HospitalId,
                    AppointmentDate = x.AppointmentDate,
                    AppointmentTime = x.AppointmentTime,
                    ApprovalStatus = x.ApprovalStatus,

                }).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ApproveAppointment(int appointmentId)
        {
            try
            {
                var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(x => x.Id == appointmentId);
                appointment.ApprovalStatus = Utility.Status.Approved;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RejectAppointment(int appointmentId)
        {
            try
            {
                var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(x => x.Id == appointmentId);
                appointment.ApprovalStatus = Utility.Status.Rejected;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AppointmentVM> GetById(int Id)
        {
            try
            {
                var query = _dbContext.Appointments.Where(x => x.Id == Id);
                return await query.Select(x => new AppointmentVM
                {
                    Id = x.Id,
                    DoctorId = x.DoctorId,
                    DoctorName = x.Doctor.FirstName + " " + x.Doctor.LastName,
                    DepartmentName = x.Doctor.Department.Name ?? "General Practician",
                    HospitalName = x.Doctor.Hospital.Name,
                    DepartmentId = x.Doctor.DepartmentId,
                    HospitalId = x.Doctor.HospitalId,
                    AppointmentDate = x.AppointmentDate,
                    AppointmentTime = x.AppointmentTime,
                    ApprovalStatus = x.ApprovalStatus,

                }).FirstOrDefaultAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Add(AppointmentVM appointment)
        {
            try
            {
                var add = new Appointment()
                {
                    DoctorId = appointment.DoctorId,
                    PatientId = appointment.PatientId,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    ApprovalStatus = Utility.Status.Pending,
                };
                await _dbContext.AddAsync(add);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Update(AppointmentVM appointment)
        {
            try
            {
                var model = await _dbContext.Appointments.FirstOrDefaultAsync(x => x.Id == appointment.Id);
                model.DoctorId = appointment.DoctorId;
                model.AppointmentDate = appointment.AppointmentDate;
                model.AppointmentTime = appointment.AppointmentTime;
                model.ApprovalStatus = appointment.ApprovalStatus;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
