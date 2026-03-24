using Discount.API.Dtos;
using Discount.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/discount")]
    [ApiController]
    //8002
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _service;

        public DiscountController(IDiscountService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDiscounts()
        {
            var response = await _service.GetAllAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDiscountById(string Id)
        {
            var response = await _service.GetByIdAsync(Id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount([FromBody] CouponDto coupon)
        {
            var response = await _service.CreateAsync(coupon);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount(string id, [FromBody] CouponDto coupon)
        {
            var response = await _service.UpdateAsync(id, coupon);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(string id)
        {
            var response = await _service.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
