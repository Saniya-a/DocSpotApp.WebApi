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
    //[Authorize(Roles = "Admin")]
    public class DepartmentController : ControllerBase
    {
        IGenericRepository<Department> _repository;
        public DepartmentController( IGenericRepository<Department> repository )
        {
            _repository = repository;
        }


        [HttpGet]
        [Route("get-all")]
        public async Task<ActionResult<List<Department>>> GetAll()
        {
            try
            {
                var list = _repository.GetAll((x => x.IsDeleted == false), (query =>
                {
                    return query.OrderBy(item => item.Name);
                })).ToList();

                return list;
            }
            catch (Exception)
            {

                return BadRequest(new Response() { Status = "Failed", Message="Some error occured while fetching departments!"});
            }
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<Department>> GetById(int id)
        {
            try
            {
                var item = await _repository.GetById(id);

                return item;
            }
            catch (Exception)
            {
                return BadRequest(new Response() { Status = "Failed", Message = "Some error occured while fetching departments!" });
            }
        }


        [HttpPost]
        [Route("add-department")]
        public async Task<IActionResult> Add([FromBody] DepartmentVM model)
        {
            var exists =  _repository.GetAll(x => !x.IsDeleted && x.Name == model.Name).FirstOrDefault();
            if (exists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Department already exists!" });

            Department add = new()
            {
               Name = model.Name
            };
            var result = await _repository.Add(add);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Department creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "Department created successfully!" });
        }

        [HttpPut]
        [Route("edit-department")]
        public async Task<IActionResult> Edit([FromBody] DepartmentVM model)
        {
            
            Department edit = new()
            {
                Id = model.Id,
                Name = model.Name
            };
            var result = await _repository.Update(edit);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Department update failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "Department updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var delete = await _repository.GetById(id);
            delete.IsDeleted = true;
            var result = await _repository.Update(delete);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Department delete failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "Department deleted successfully!" });
        }
    }
}
