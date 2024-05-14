using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using fw_shop_api.Models.Util;

namespace fw_shop_api.Data.Interfaces
{
    public interface IApplicationAuthService
    {
        Task<BaseResponse<User>> Registration(RegisterUserRequestDto model);
    }
}