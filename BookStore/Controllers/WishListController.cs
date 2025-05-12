using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishlistBL _wishlistBL;
        public WishListController(IWishlistBL wishlistBL)
        {
            _wishlistBL = wishlistBL;
        }
        [HttpPost("WishlistBook")]
        public async Task<IActionResult> WishlistBook(int bookId)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                var result = await _wishlistBL.WishlistBook(bookId, userId);
                if (result.Success == true)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("GetAllWishlistedBooks")]
        public async Task<IActionResult> GetAllWishlistedBooks()
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                var result = await _wishlistBL.GetAllWishlistedBooks(userId);
                if (result.Success == true)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpDelete("RemoveBookFromWishlist")]
        public async Task<IActionResult> RemoveBookFromWishlist(int bookId)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                var result = await _wishlistBL.RemoveBookFromWishlist(bookId, userId);
                if (result.Success == true)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpDelete("ClearWishlist")]
        public async Task<IActionResult> ClearWishlist()
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                var result = await _wishlistBL.ClearWishlist(userId);
                if (result.Success == true)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
