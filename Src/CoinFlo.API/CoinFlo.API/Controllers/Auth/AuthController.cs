using CoinFlo.API.Helpers;
using CoinFlo.BLL.Exceptions.Auth;
using CoinFlo.BLL.IRepository.IAuthRepository;
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
        private readonly IAuthRepository _authRepository;
        private readonly JwtTokenGenerator _jwtGenerator;

        public AuthController(IUsersRepository usersRepository, IAuthRepository authRepository, JwtTokenGenerator jwtGenerator)
        {
            _authRepository = authRepository;
            _jwtGenerator = jwtGenerator;
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistration(Users user)
        {
            try
            {
                await _authRepository.UserSignUp(user);
                return ResponseHelper.GetActionResponse(true, "User Registration Success", user);
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
        public async Task<IActionResult> UserLogin([FromBody] LoginRequest loginRequest)
        {
            try
            {
                LoginResponse loginResponse = await _authRepository.UserLogin(loginRequest.email, loginRequest.password);

                if (loginResponse == null)
                {
                    return ResponseHelper.GetActionResponse(false, "Invalid Credentials");
                }
                if (loginResponse.UserStatus == false)
                {
                    return ResponseHelper.GetActionResponse(false, "Sorry. Your account is disabled. For more information please contact admin");
                }

                string jwtToken = _jwtGenerator.GetJwtToken(loginResponse);
                string refreshToken = _jwtGenerator.GenerateRefreshToken();
                Users user = await _authRepository.GetCurrentLoggedinUserData(loginResponse.Id, loginResponse.UserSecretKey);
                ResponseHelper.StoreLoggedinUserIdKey(Response, loginResponse);
                await _authRepository.SaveRefreshToken(loginResponse.Id, refreshToken);

                var userLoginData = new 
                {
                    Token = jwtToken, 
                    RefreshToken = refreshToken, 
                    User = user
                };

                return ResponseHelper.GetActionResponse(true, "Valid User", userLoginData);
            }
            catch(Exception ex)
            {
                return ResponseHelper.GetActionResponse(false, $"Internal Server Error. Error Message : {ex.Message}");
            }
        }


        [HttpPost("OTP-Verification")]
        public async Task<IActionResult> OTPVerification([FromBody] OTPVerificationRequest otpVerificationRequest)
        {
            try
            {
                var otpVerificationResponse = await _authRepository.OTPVerification(otpVerificationRequest.otpCode, otpVerificationRequest.userEmail);
                return ResponseHelper.GetActionResponse(otpVerificationResponse.otpStatus, otpVerificationResponse.otpMessage, otpVerificationRequest.otpCode);
            }
            catch (Exception ex)
            {
                return ResponseHelper.GetActionResponse(false, $"Internal Server Error. Message : {ex.Message}");
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            var principal = _jwtGenerator.GetPrincipalFromExpiredToken(tokenRequest.Token);
            if (principal == null)
            {
                return ResponseHelper.GetActionResponse(false, "Invalid token");
            }

            var userId = principal.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var savedRefreshToken = await _authRepository.GetRefreshToken(int.Parse(userId), tokenRequest.Token);

            if (savedRefreshToken == null)
            {
                return ResponseHelper.GetActionResponse(false, "Invalid refresh token");
            }

            var newJwtToken = _jwtGenerator.GetJwtToken(new LoginResponse
            {
                Id = int.Parse(userId),
                UserSecretKey = principal.Claims.FirstOrDefault(c => c.Type == "UserSecretKey")?.Value
            });

            var newRefreshToken = _jwtGenerator.GenerateRefreshToken();
            await _authRepository.UpdateRefreshToken(int.Parse(userId), tokenRequest.Token, newRefreshToken);

            return ResponseHelper.GetActionResponse(true, "Token refreshed", new { Token = newJwtToken, RefreshToken = newRefreshToken });
        }


    }
}
