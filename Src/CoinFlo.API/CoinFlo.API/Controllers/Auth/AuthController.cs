using CoinFlo.API.Helpers;
using CoinFlo.BLL.Exceptions.Auth;
using CoinFlo.BLL.IRepository.IUsersRepository;
using CoinFlo.BLL.Models.Auth;
using CoinFlo.BLL.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Cryptography;

namespace CoinFlo.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly JwtTokenGenerator _jwtGenerator;

        public AuthController(IUsersRepository usersRepository, JwtTokenGenerator jwtGenerator)
        {
            _usersRepository = usersRepository;
            _jwtGenerator = jwtGenerator;
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


        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin(string email, string password)
        {
            try
            {
                LoginResponse loginResponse = await _usersRepository.UserLogin(email, password);

                if (loginResponse == null)
                {
                    return StatusCode(500, $"Invalid Credential");
                }
                if (loginResponse.UserStatus == false)
                {
                    return StatusCode(500, $"Sorry. Your account is disabled. For more information please contact admin");
                }

                string jwtToken = _jwtGenerator.GetJwtToken(loginResponse);
                Users user = await _usersRepository.GetCurrentLoggedinUserData(loginResponse.Id, loginResponse.UserSecretKey);
                ResponseHelper.StoreLoggedinUserIdKey(Response, loginResponse);

                return Ok(new { Token = jwtToken, User = user });
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Error Message : {ex.Message}");
            }
        }


        [HttpPost("OTP-Verification")]
        public async Task<IActionResult> OTPVerification(int otpCode, string userEmail)
        {
            try
            {
                var otpVerificationResponse = await _usersRepository.OTPVerification(otpCode, userEmail);
                return ResponseHelper.GetActionResponse(otpVerificationResponse.otpStatus, otpVerificationResponse.otpMessage, otpCode);
            }
            catch (Exception ex)
            {
                return ResponseHelper.GetActionResponse(false, $"Internal Server Error. Message : {ex.Message}");
            }
        }
    }
}
