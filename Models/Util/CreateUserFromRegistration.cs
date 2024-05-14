using fw_shop_api.Data.App;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using fw_shop_api.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Extensions;

namespace fw_shop_api.Models.Util
{
    public static class CreateUserFromRegistration
    {
        public static async Task<User> CreateNewUser(this UserManager<User> userManager, AuthDbContext context, RegisterUserRequestDto model, LoginProvider loginProvider)
        {
            var user = await userManager.FindByLoginAsync(loginProvider.GetDisplayName(), model.LoginProviderSubject);

            if (user is not null) return user;

            user = await userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email
                };

                await userManager.CreateAsync(user, model.Password);
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            }

            var userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
            var result = await userManager.AddLoginAsync(user, userLoginInfo);

            if (result.Succeeded) return user;
            else return null;
        }
    }
}