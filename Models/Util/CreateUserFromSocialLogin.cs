using fw_shop_api.Data.App;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using fw_shop_api.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Extensions;

namespace fw_shop_api.Models.Util
{
    public static class CreateUserFromSocialLogin
    {
        public static async Task<User> CreateUserFromSocial(this UserManager<User> userManager, AuthDbContext context, CreateUserFromSocialDto model, LoginProvider loginProvider)
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
                    UserName = model.Email,
                    ProfilePicture = model.ProfilePicture
                };

                await userManager.CreateAsync(user);
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            }

            UserLoginInfo userLoginInfo = null;
            switch (loginProvider)
            {
                case LoginProvider.Google:
                {
                    userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
                }
                break;
                case LoginProvider.Facebook:
                {
                    userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
                }
                break;
                default: break;
            }

            var result = await userManager.AddLoginAsync(user, userLoginInfo!);

            if (result.Succeeded) return user;
            else return null;
        }
    }
}