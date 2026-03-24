using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.API.Dtos;
using User.API.Services.Employee;

namespace User.API.Controllers.Employee
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetById()
        {
            var IdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var response = await _service.GetByIdAsync(int.Parse(IdClaim!.Value));
            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
          
            var response = await _service.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto user)
        {
            var response = await _service.UpdateAsync(id, user);
            return StatusCode(response.StatusCode, response);
        }

    }
}