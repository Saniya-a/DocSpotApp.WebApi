using AppAPI.Auth;
using DocSpotApp.Models;
using DocSpotApp.Repository.DAL.Interfaces;
using DocSpotApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocSpotApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        IApplicationUserRepository _userRepository;
        public PatientController( IApplicationUserRepository userRepository )
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        [Route("get-all")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<PatientVM>>> GetAll()
        {
            try
            {
                var list = await _userRepository.GetPatientsAsync();

                return list;
            }
            catch (Exception)
            {

                return BadRequest(new Response() { Status = "Failed", Message="Some error occured while fetching departments!"});
            }
        }

        [HttpGet]
        [Route("get")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<PatientVM>> GetById(string id)
        {
            try
            {
                var item = await _userRepository.GetByIdAsync(id);
                var patient = new PatientVM(item);
                return patient;
            }
            catch (Exception)
            {
                return BadRequest(new Response() { Status = "Failed", Message = "Couldn't find the patient! Try again!" });
            }
        }

        [HttpPut]
        [Route("edit/{id}")]
        //[Authorize(Roles = "Admin, Patient")]

        public async Task<IActionResult> EditPatient([FromBody] PatientVM model)
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
                UserName = model.Username
            };

            var success = await _userRepository.UpdateAsync(edit);
            if (!success)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to update user details!" });

            return Ok(new Response { Status = "Success", Message = "User details updated successfully!" });
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (result)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Patient delete failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "Patient deleted successfully!" });
        }
    }
}
