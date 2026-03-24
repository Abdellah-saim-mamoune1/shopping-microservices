using Basket.API.Dtos;
using Basket.API.Entities;
using Basket.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Basket.API.Controllers2
{
    [Authorize(Roles = "Client")]
    [Route("api/v1/basket")]
    [ApiController]
    //8001
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService
                ?? throw new ArgumentNullException(nameof(basketService));
        }

    
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var response = await _basketService.GetAsync(GetUserId());
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("item/")]
        public async Task<IActionResult> AddItem([FromBody] CartItem item)
        {
            var response = await _basketService.AddItemAsync(item, GetUserId());
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("item")]
        public async Task<IActionResult> UpdateItem([FromBody] BasketUpdateDto item)
        {
            var response = await _basketService.UpdateItemAsync(item, GetUserId());
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("item/{ProductId}")]
        public async Task<IActionResult> DeleteItem(string ProductId)
        {
            var response = await _basketService.DeleteItemAsync(ProductId, GetUserId());
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutDto basketCheckout)
        {

           var response= await _basketService.CheckoutItemAsync(basketCheckout, int.Parse(GetUserId()));

            return StatusCode(response.StatusCode, response);
        }

        private string GetUserId()
        {
            var clientIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return clientIdClaim!.Value;
        }
    }
}
