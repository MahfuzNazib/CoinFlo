using CoinFlo.BLL.Models.CommonDTO;
using CoinFlo.BLL.Models.PayType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.IRepository.IPayTypeRepository
{
    public interface IPayTypeRepository
    {
        Task<IEnumerable<PayTypes>> GetAllPayTypes(UserDto userDto);

        Task CreateNewPayType(PayTypes payType);
    }
}
