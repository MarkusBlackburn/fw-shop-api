using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using fw_shop_api.configs;
using fw_shop_api.Data.App;
using fw_shop_api.Data.Interfaces;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using fw_shop_api.Models.Util;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace fw_shop_api.Data.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _context;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly UserManager<User> _userManager;
        private readonly IOptions<Jwt> _jwt;

        public AuthService(
            AuthDbContext context,
            IGoogleAuthService googleAuthService,
            UserManager<User> userManager,
            IOptions<Jwt> jwt)
        {
            _context = context;
            _googleAuthService = googleAuthService;
            _userManager = userManager;
            _jwt = jwt;
        }
        public async Task<BaseResponse<JwtResponseVM>> SignInWithGoogle(GoogleSignInVM model)
        {
            var response = await _googleAuthService.GoogleSignIn(model);

            if (response.Errors.Any()) return new BaseResponse<JwtResponseVM>(response.ResponseMessage, response.Errors);

            var jwtResponse = CreateJwtToken(response.Data);
            var data = new JwtResponseVM
            {
                Token = jwtResponse
            };

            return new BaseResponse<JwtResponseVM>(data);
        }

        private string CreateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_jwt.Value.Secret);
            var userClaims = BuildUserClaims(user);
            var signKey = new SymmetricSecurityKey(key);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Value.ValidIssuer,
                notBefore: DateTime.UtcNow,
                audience: _jwt.Value.ValidAudience,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwt.Value.DurationInMinutes)),
                claims: userClaims,
                signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256));
            
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private List<Claim> BuildUserClaims(User user)
        {
            var userClaims = new List<Claim>()
            {
                new(JwtClaimTypes.Id, user.Id.ToString()),
                new(JwtClaimTypes.Email, user.Email!),
                new(JwtClaimTypes.GivenName, user.FirstName),
                new(JwtClaimTypes.FamilyName, user.LastName),
                new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            return userClaims;
        }
    }
}