using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.DTO;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressBL _addressBL;
        public AddressController(IAddressBL addressBL)
        {
            _addressBL = addressBL;
        }

        [Authorize]
        [HttpPost("AddAddress")]
        public async Task<IActionResult> AddAddress(UserAddressReqDTO request)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                var result = await _addressBL.AddAddressAsync(request, userId);
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
        [HttpPut("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress(UserAddressReqDTO request, int addressId)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                var result = await _addressBL.UpdateAddressAsync(request, addressId, userId);
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
        [HttpDelete("DeleteAddress")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                var result = await _addressBL.DeleteAddressAsync(addressId, userId);
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
        [HttpGet("GetAllAddresses")]
        public async Task<IActionResult> GetAllAddresses()
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
                var result = await _addressBL.GetAllAddressesAsync(userId);
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
