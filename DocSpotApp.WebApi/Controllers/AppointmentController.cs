using AppAPI.Auth;
using DocSpotApp.Models;
using DocSpotApp.Repository.DAL.Interfaces;
using DocSpotApp.Repository.DAL.Repositories;
using DocSpotApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocSpotApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;
        public AppointmentController(IAppointmentRepository repository)
        {
            _repository = repository;
        }
        [Authorize(Roles = "Patient")]
        [HttpGet]
        [Route("patient-appointments/{patientId}")]
        public async Task<ActionResult<List<AppointmentVM>>> GetAppointmentsByPatientId(string patientId)
        {
            try
            {
                var appointments = await _repository.GetAppointmentsByPatientId(patientId);
                return appointments;
            }
            catch (Exception)
            {
                return BadRequest(new Response { Status = "Failed", Message = "Some error occurred while fetching appointments!" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("get-all")]
        public async Task<ActionResult<List<AppointmentVM>>> GetAllAppointments()
        {
            try
            {
                var appointments = await _repository.GetAll();
                return appointments;
            }
            catch (Exception)
            {
                return BadRequest(new Response { Status = "Failed", Message = "Some error occurred while fetching appointments!" });
            }
        }


        [HttpGet]
        [Route("doctor-appointments/{doctorId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<List<AppointmentVM>>> GetAppointmentsByDoctorId(string doctorId)
        {
            try
            {
                var appointments = await _repository.GetAppointmentsByDoctorId(doctorId);
                return appointments;
            }
            catch (Exception)
            {
                return BadRequest(new Response { Status = "Failed", Message = "Some error occurred while fetching appointments!" });
            }
        }

        [HttpGet]
        [Route("approve/{appointmentId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<Response>> ApproveAppointment(int appointmentId)
        {
            try
            {
                await _repository.ApproveAppointment(appointmentId);
                return Ok(new Response { Status = "Success", Message = "Appointment approved successfully!" });
            }
            catch (Exception)
            {
                return BadRequest(new Response { Status = "Failed", Message = "Some error occurred while approving appointment!" });
            }
        }

        [HttpGet]
        [Route("reject/{appointmentId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<Response>> RejectAppointment(int appointmentId)
        {
            try
            {
                await _repository.RejectAppointment(appointmentId);
                return Ok(new Response { Status = "Success", Message = "Appointment rejected successfully!" });
            }
            catch (Exception)
            {
                return BadRequest(new Response { Status = "Failed", Message = "Some error occurred while rejecting appointment!" });
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<AppointmentVM>> GetAppointmentById(int id)
        {
            try
            {
                var appointment = await _repository.GetById(id);
                return Ok(appointment);
            }
            catch (Exception)
            {
                return BadRequest(new Response { Status = "Failed", Message = "Some error occurred while fetching appointment!" });
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<Response>> AddAppointment([FromBody] AppointmentVM appointment)
        {
            try
            {
                await _repository.Add(appointment);
                return Ok(new Response { Status = "Success", Message = "Appointment added successfully!" });
            }
            catch (Exception)
            {
                return BadRequest(new Response { Status = "Failed", Message = "Some error occurred while adding appointment!" });
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<Response>> UpdateAppointment([FromBody] AppointmentVM appointment)
        {
            try
            {
                await _repository.Update(appointment);
                return Ok(new Response { Status = "Success", Message = "Appointment updated successfully!" });
            }
            catch (Exception)
            {
                return BadRequest(new Response { Status = "Failed", Message = "Some error occurred while updating appointment!" });
            }
        }


    }
}
