using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.API.Services.Employee;

namespace User.API.Controllers.Employee
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/employee/clients")]
    [ApiController]
    public class EmployeeClientController : ControllerBase
    {
        private readonly IEmployeeClientService _service;

        public EmployeeClientController(IEmployeeClientService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}