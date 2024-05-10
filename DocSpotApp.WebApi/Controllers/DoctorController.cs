using AppAPI.Auth;
using DocSpotApp.Models;
using DocSpotApp.Repository.DAL.Interfaces;
using DocSpotApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Numerics;

namespace DocSpotApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        IApplicationUserRepository _userRepository;
        public DoctorController(IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        [Route("get-all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<DoctorVM>>> GetAll()
        {
            try
            {
                var list = await _userRepository.GetDoctorssAsync();
                return list;
            }
            catch (Exception)
            {

                return BadRequest(new Response() { Status = "Failed", Message = "Some error occured while fetching departments!" });
            }
        }

        [HttpGet]
        [Route("get")]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<ActionResult<DoctorVM>> GetById(string id)
        {
            try
            {
                var item = await _userRepository.GetByIdAsync(id);
                var doctor = new DoctorVM(item);
                return doctor;
            }
            catch (Exception)
            {
                return BadRequest(new Response() { Status = "Failed", Message = "Couldn't find the patient! Try again!" });
            }
        }

        [HttpPut]
        [Route("edit")]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> Edit([FromBody] DoctorVM model)
        {
            var edit = new ApplicationUser()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                DOB = model.DOB,
                Email = model.Email,
                PhoneNumber = model.Mobile,
                UserName = model.Username,
                HospitalId = model.HospitalId,
                DepartmentId = model.DepartmentId,
                Fees = model.Fees,
            };

            var success = await _userRepository.UpdateAsync(edit);
            if (!success)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to update doctor details!" });

            return Ok(new Response { Status = "Success", Message = "Doctor details updated successfully!" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (result)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Doctor delete failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "Doctor deleted successfully!" });
        }

        [Authorize]
        [HttpGet]
        [Route("get-doctorList")]
        public async Task<ActionResult<List<DoctorVM>>> GetDoctorList(int hospitalId, int departmentId)
        {
            var result = await _userRepository.GetDoctorListAsync(hospitalId, departmentId);
            return result;
        }

    }
}
