using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.DTO;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookBL _bookBL;
        public BookController(IBookBL bookBL)
        {
            _bookBL = bookBL;
        }

        [Authorize]
        [HttpPost("UploadBook")]
        public async Task<IActionResult> UploadBook(AddBookReqDTO request)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                var result = await _bookBL.UploadBookAsync(request,userId);
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

        [Authorize]
        [HttpGet("GetBookById")]
        public async Task<IActionResult> ViewBookById(int bookId)
        {
            try
            {
                var result = await _bookBL.ViewBookByIdAsync(bookId);
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

        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var result = await _bookBL.GetAllBooksAsync();
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
