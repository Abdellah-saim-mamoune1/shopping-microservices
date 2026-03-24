using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.API.Dtos;
using User.API.Services.Client;

namespace User.API.Controllers.Client
{
    [Authorize(Roles = "Client")]
    [Route("api/v1/client")]
    [ApiController]
    //8005
    public class ClientController : ControllerBase
    {
        private readonly IClientService _service;

        public ClientController(IClientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetById()
        {

            var response = await _service.GetByIdAsync(GetUserId());
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> Update( UserUpdateDto user)
        {
            var response = await _service.UpdateByIdAsync(GetUserId(), user);
            return StatusCode(response.StatusCode, response);
        }


        private int GetUserId()
        {
            var clientIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(clientIdClaim!.Value);
        }
    }
}