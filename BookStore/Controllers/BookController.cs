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
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                string fileName = null;

                
                if (request.BookImage != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(request.BookImage.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest(new ResponseDTO<string>
                        {
                            Success = false,
                            Message = "Invalid file type. Allowed types are: " + string.Join(", ", allowedExtensions)
                        });
                    }

                    if (request.BookImage.Length > 5 * 1024 * 1024) 
                    {
                        return BadRequest(new ResponseDTO<string>
                        {
                            Success = false,
                            Message = "File size exceeds the 5 MB limit."
                        });
                    }

                    
                    fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "bookstore/images");

                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    var filePath = Path.Combine(imagePath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.BookImage.CopyToAsync(stream);
                    }
                }
                else
                {
                    return BadRequest(new ResponseDTO<string>
                    {
                        Success = false,
                        Message = "Book image is required."
                    });
                }

                
                var result = await _bookBL.UploadBookAsync(request, userId,fileName);

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
                return StatusCode(500, new
                {
                    message = ex.Message
                });
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
