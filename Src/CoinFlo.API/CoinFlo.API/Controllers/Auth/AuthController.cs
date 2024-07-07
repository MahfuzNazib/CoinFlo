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
                //int length = 5;
                //for (int i=0; i<100000; i++)
                //{
                //    user.UserSecretKey = Guid.NewGuid();
                //    user.FirstName = GenerateRandomString(length);
                //    user.LastName = GenerateRandomString(length);
                //    user.Email = $"{user.FirstName}_{user.LastName}@gmail.com";
                //    user.Password = GenerateRandomString(length);

                //    await _usersRepository.UserSignUp(user);
                //}

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

        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            char[] result = new char[length];
            byte[] buffer = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(buffer);
            }

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[buffer[i] % chars.Length];
            }

            return new string(result);
        }
    }
}
