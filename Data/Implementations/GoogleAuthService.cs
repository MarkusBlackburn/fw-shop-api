using fw_shop_api.configs;
using fw_shop_api.Data.App;
using fw_shop_api.Data.Interfaces;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using fw_shop_api.Models.Enums;
using fw_shop_api.Models.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace fw_shop_api.Data.Implementations
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthDbContext _context;
        private readonly GoogleAuthConfig _config;

        public GoogleAuthService(
            UserManager<User> userManager,
            AuthDbContext context,
            IOptions<GoogleAuthConfig> config
            )
        {
            _userManager = userManager;
            _context = context;
            _config = config.Value;
        }
        public async Task<BaseResponse<User>> GoogleSignIn(GoogleSignInVM model)
        {
            Payload payload= new();

            try
            {
                payload = await ValidateAsync(model.IdToken, new ValidationSettings
                {
                    Audience = new[] { _config.ClientId }
                });
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>(null, new List<string> {"Failed to get a response"});
            }

            var userCreated = new CreateUserFromSocialDto
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                ProfilePicture = payload.Picture,
                LoginProviderSubject = payload.Subject
            };

            var user = await _userManager.CreateUserFromSocial(_context, userCreated, LoginProvider.Google);

            if (user is not null) return new BaseResponse<User>(user);
            else return new BaseResponse<User>(null, new List<string> {"Unable to link a Local User to a Provider"});
        }
    }
}