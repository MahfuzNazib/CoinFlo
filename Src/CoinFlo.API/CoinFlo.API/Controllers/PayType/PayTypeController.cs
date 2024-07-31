using CoinFlo.API.Helpers;
using CoinFlo.BLL.IRepository.IPayTypeRepository;
using CoinFlo.BLL.Models.CommonDTO;
using CoinFlo.BLL.Models.PayType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoinFlo.API.Controllers.PayType
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayTypeController : BaseController
    {
        public readonly IPayTypeRepository _payTypeRepository;

        public PayTypeController(IPayTypeRepository payTypeRepository)
        {
            _payTypeRepository = payTypeRepository;
        }


        [HttpPost("PayTypes")]
        public async Task<IActionResult> GetAllPayTypes(UserDto userDto)
        {
            try
            {
                var payTypes = await _payTypeRepository.GetAllPayTypes(userDto);

                if (payTypes == null)
                {
                    return NotFound();
                }

                return Ok(payTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Faild to Fetch All Pay Types Data. Error Message : {ex.Message}");
            }
        }


        [HttpPost("CreateNewPayType")]
        public async Task<IActionResult> CreateNewPayType(PayTypes payType)
        {
            try
            {
                payType.UserId = userId;
                payType.UserKey = userSecretKey;

                await _payTypeRepository.CreateNewPayType(payType);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. ({ex.Message})");
            }
        }
    }
}
