using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.DTO;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartBL cartBL;
        public CartController(ICartBL cartBL)
        {
            this.cartBL = cartBL;
        }

        [Authorize]
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(AddToCartReqDTO addToCartReqDTO)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                var result = await cartBL.AddToCartAsync(addToCartReqDTO, userId);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value); var result = await cartBL.RemoveFromCartAsync(cartId, userId);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetAllCartItems")]
        public async Task<IActionResult> GetAllCartItems()
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

                var result = await cartBL.GetAllCartItemsAsync(userId);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCart(int cartId, int quantity)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

                var result = await cartBL.UpdateCartAsync(cartId, quantity, userId);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);

                var result = await cartBL.ClearCartAsync(userId);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
