using CoinFlo.BLL.IRepository.IPayTypeRepository;
using CoinFlo.BLL.Models.CommonDTO;
using CoinFlo.BLL.Models.PayType;
using CoinFlo.DAL.DapperDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Repository.PayTypeRepository
{
    public class PayTypeRepository : IPayTypeRepository
    {
        private readonly IDapperDataAccess _dapperDataAccess;

        public PayTypeRepository(IDapperDataAccess dapperDataAccess)
        {
            _dapperDataAccess = dapperDataAccess;
        }


        public async Task<IEnumerable<PayTypes>> GetAllPayTypes(UserDto userDto)
        {
            string query = "SP_GET_ALL_PAY_TYPES";
            var parameters = new
            {
                Id = userDto.Id,
                UserId = userDto.UserId,
                UserKey = userDto.UserKey
            };

            return await _dapperDataAccess.GetData<PayTypes, dynamic>(query, parameters);    
        }


        public async Task CreateNewPayType(PayTypes payType)
        {
            string query = "SP_PAYTYPE_CREATE_NEW";
            var parameters = new 
            { 
                UserId = payType.UserId,
                UserKey = payType.UserKey,
                PayBy = payType.PayBy,
                Status = payType.Status,
            };

            await _dapperDataAccess.InsertData(query, parameters);
        }
    }
}
