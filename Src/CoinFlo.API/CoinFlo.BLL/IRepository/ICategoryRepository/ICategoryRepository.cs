using CoinFlo.BLL.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.IRepository.ICategoryRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Categories>> GetAllCategories(int userId, string userSecretKey);

        Task CreateNewCategory(Categories category);

        Task<Categories> GetCategoryDetails(int id, int userId, string userSecretKey);

        Task UpdateCategory(Categories category);

        Task DeleteCategory(int id, int userId, string userSecretKey);
    }
}
