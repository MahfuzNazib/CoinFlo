using CoinFlo.BLL.IRepository.IUsersRepository;
using CoinFlo.BLL.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoinFlo.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsersRepository _userRepository;
        private readonly int userId = 2;
        private readonly string userSecretKey = "0E033FB3-ABA9-4D04-898F-087DAD87F777";

        public UserController(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("UserProfile")]
        public async Task<IActionResult> GetCurrentLoggedInUserProfile()
        {
            try
            {
                // Call a method who can send the logged in user Id & UserSecretKey;

                Users user = await _userRepository.GetCurrentLoggedinUserData(userId, userSecretKey);
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
