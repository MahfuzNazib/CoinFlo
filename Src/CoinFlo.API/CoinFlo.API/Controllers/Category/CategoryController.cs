using CoinFlo.API.Helpers;
using CoinFlo.BLL.IRepository.ICategoryRepository;
using CoinFlo.BLL.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoinFlo.API.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [Authorize]
        [HttpGet("Categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategories(userId, userSecretKey);

                if (categories == null)
                {
                    return ResponseHelper.GetActionResponse(true, "No Category Data Found", null);
                }
                return ResponseHelper.GetActionResponse(true, "Successfully Fetch Category Data", categories);
            }
            catch (Exception ex)
            {
                return ResponseHelper.GetActionResponse(false, $"Faild to Fetch Category Data. Error Message : {ex.Message}", null);
            }
        }


        [HttpPost("CreateNewCategory")]
        public async Task<IActionResult> CreateNewCategory(Categories category)
        {
            try
            {
                category.UserId = userId;
                category.UserKey = userSecretKey;

                await _categoryRepository.CreateNewCategory(category);
                return ResponseHelper.GetActionResponse(true, "Successfully Create New Catroty", category);
            }
            catch (Exception ex)
            {
                return ResponseHelper.GetActionResponse(false, $"Internal Server Error to Create New Category. Error Message : {ex.Message}", null);
            }
        }


        [HttpPost("GetCategoryDetails")]
        public async Task<IActionResult> GetCategoryDetails(int id)
        {
            try
            {
                Categories category = await _categoryRepository.GetCategoryDetails(id, userId, userSecretKey);
                if (category == null)
                {
                    return NotFound();
                }
                return ResponseHelper.GetActionResponse(true, "Successfully Fetch Category Grid", category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Faild to Fetch Category Details. Error Message : {ex.Message}");
            }
        }


        [HttpPost("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(Categories category)
        {
            try
            {
                category.UserId = userId;
                category.UserKey = userSecretKey;
                await _categoryRepository.UpdateCategory(category);

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Update Faild. Error Message : {ex.Message}");
            }
        }


        [HttpPost("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategory(id, userId, userSecretKey);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Faild to Delete Category. Error Message : {ex.Message}");
            }
        }
    }
}
