using CoinFlo.BLL.IRepository.IAuthRepository;
using CoinFlo.BLL.IRepository.IUsersRepository;
using CoinFlo.BLL.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoinFlo.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUsersRepository _userRepository;
        private readonly IAuthRepository _authRepository;

        public UserController(IUsersRepository userRepository, IAuthRepository authRepository)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
        }

        [HttpGet("UserProfile")]
        public async Task<IActionResult> GetCurrentLoggedInUserProfile()
        {
            try
            {
                Users user = await _authRepository.GetCurrentLoggedinUserData(userId, userSecretKey);
                if (user == null) 
                { 
                    return NotFound();
                }
                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error to Fetch User Data. Error Message : {ex.Message}");
            }
        }

        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateUserProfile(Users user)
        {
            try
            {
                await _userRepository.UpdateUserProfile(userId, userSecretKey, user);
                return Ok("User Profule Update Success");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error to Update User Profile. Error Message : {ex.Message}");
            }
        }
    }
}
