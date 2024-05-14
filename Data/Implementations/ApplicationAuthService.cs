using fw_shop_api.Data.App;
using fw_shop_api.Data.Interfaces;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using fw_shop_api.Models.Enums;
using fw_shop_api.Models.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace fw_shop_api.Data.Implementations
{
    public class ApplicationAuthService : IApplicationAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthDbContext _context;

        public ApplicationAuthService(
            UserManager<User> userManager,
            AuthDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<BaseResponse<User>> Registration(RegisterUserRequestDto model)
        {
            var userCreated = new RegisterUserRequestDto
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var user = await _userManager.CreateNewUser(_context, userCreated, LoginProvider.Application);

            if (user is not null) return new BaseResponse<User>(user);
            else return new BaseResponse<User>(null, ["Enable to create new user"]);
        }
    }
}