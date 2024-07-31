using CoinFlo.BLL.IRepository.ICategoryRepository;
using CoinFlo.BLL.Models.Category;
using CoinFlo.DAL.DapperDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Repository.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDapperDataAccess _dapperDataAccess;

        public CategoryRepository(IDapperDataAccess dapperDataAccess)
        {
            _dapperDataAccess = dapperDataAccess;
        }

        public async Task<IEnumerable<Categories>> GetAllCategories(int userId, string userSecretKey)
        {
            string SP_NAME = "SP_GET_ALL_CATEGORIES";
            var parameters = new
            {
                UserId = userId,
                UserKey = userSecretKey
            };

            
            var categories = await _dapperDataAccess.GetData<Categories, dynamic>(SP_NAME, parameters);
            return categories;

            //return await _dapperDataAccess.GetData<Categories, dynamic>(SP_NAME, parameters);
        }

        public async Task CreateNewCategory(Categories category)
        {
            string query = "SP_CREATE_NEW_CATEGORY";
            var parameters = new
            {
                category.UserId,
                category.UserKey,
                category.Name,
                category.Type,
                category.Status,
            };

            await _dapperDataAccess.ExecuteQuery(query, parameters);
        }


        public async Task<Categories> GetCategoryDetails(int id, int userId, string userSecretKey)
        {
            string query = "SP_GET_CATEGORY_DETAILS";
            var parameters = new
            {
                Id = id,
                UserId = userId,
                UserKey = userSecretKey
            };

            return await _dapperDataAccess.GetSingleData<Categories, dynamic>(query, parameters);
        }


        public async Task UpdateCategory(Categories category)
        {
            string query = "SP_UPDATE_CATEGORY";
            var parameters = new
            {
                category.Id,
                category.UserId,
                category.UserKey,
                category.Name,
                category.Type,
                category.Status,
            };

            await _dapperDataAccess.ExecuteQuery(query, parameters);
        }


        public async Task DeleteCategory(int id, int userId, string userSecretKey)
        {
            string query = "SP_DELETE_CATEGORY";
            var parameters = new
            {
                Id = id,
                UserId = userId,
                UserKey = userSecretKey
            };

            await _dapperDataAccess.ExecuteQuery(query, parameters);
        }
    }
}
