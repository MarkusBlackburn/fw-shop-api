using fw_shop_api.DTOs;
using fw_shop_api.Models.Util;

namespace fw_shop_api.Data.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<BaseResponse<User>> GoogleSignIn(GoogleSignInVM model);
    }
}