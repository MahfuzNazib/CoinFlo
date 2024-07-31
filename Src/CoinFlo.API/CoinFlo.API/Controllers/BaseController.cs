using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoinFlo.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        protected int userId => GetUserId();
        protected string userSecretKey => GetUserSecretKey();

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID claim.");
            }
            return userId;
        }

        private string GetUserSecretKey()
        {
            var userSecretKeyClaim = User.FindFirst("UserSecretKey");
            if (userSecretKeyClaim == null)
            {
                throw new UnauthorizedAccessException("Invalid or missing user secret key claim.");
            }
            return userSecretKeyClaim.Value;
        }
    }
}
