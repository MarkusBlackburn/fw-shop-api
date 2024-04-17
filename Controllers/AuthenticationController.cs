using fw_shop_api.Data.Interfaces;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Util;
using Microsoft.AspNetCore.Mvc;

namespace fw_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        public async Task<IActionResult> GoogleSignIn(GoogleSignInVM model)
        {
            try
            {
                return ReturnResponse(await _authService.SignInWithGoogle(model));
            }
            catch (Exception ex) { return HandleError(ex); }
        }
    }
}