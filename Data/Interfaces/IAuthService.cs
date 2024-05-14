using fw_shop_api.DTOs;
using fw_shop_api.Models.Util;

namespace fw_shop_api.Data.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResponse<JwtResponseVM>> SignInWithGoogle(GoogleSignInVM model);
        Task<BaseResponse<JwtResponseVM>> Registration(RegisterUserRequestDto model);
    }
}