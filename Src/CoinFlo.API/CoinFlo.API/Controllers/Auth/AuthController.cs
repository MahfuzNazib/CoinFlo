using CoinFlo.BLL.Exceptions.Auth;
using CoinFlo.BLL.IRepository;
using CoinFlo.BLL.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CoinFlo.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public AuthController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistration(Users user)
        {
            try
            {
                await _usersRepository.UserSignUp(user);
                return Ok(user);
            }
            catch(AuthCustomExceptions.EmailAlreadyInUseException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Error Message : {ex.Message}");
            }   
        }
    }
}
