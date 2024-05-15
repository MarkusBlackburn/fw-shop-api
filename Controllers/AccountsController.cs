using AutoMapper;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;
using fw_shop_api.Models.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace fw_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;
        private readonly IEmailSender _emailSender;

        public AccountsController(UserManager<User> userManager, IMapper mapper, JwtHandler jwtHandler, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _emailSender = emailSender;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegisterDto user)
        {
            if (user == null || !ModelState.IsValid) return BadRequest();

            var newUser = _mapper.Map<User>(user);
            var result = await _userManager.CreateAsync(newUser, user.Password!);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDto { Errors = errors});
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var param = new Dictionary<string, string?>
            {
                {"token", token},
                {"email", newUser.Email}
            };

            var callback = QueryHelpers.AddQueryString(user.ClientURI!, param);
            //var message = new Message([user.Email!], "Email Confirmation Token", callback, null);
            //await _emailSender.SendEmailAsync(message);
            await _userManager.AddToRoleAsync(newUser, "Viewer");

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthDto user)
        {
            var existUser = await _userManager.FindByNameAsync(user.Email!);

            if (existUser == null) return NotFound("User not found!");

            if (!await _userManager.IsEmailConfirmedAsync(existUser)) return Unauthorized(new AuthResponseDto { ErrorMessage = "Email isn't confirmed" });

            if (!await _userManager.CheckPasswordAsync(existUser, user.Password!))
            {
                await _userManager.AccessFailedAsync(existUser);

                if (await _userManager.IsLockedOutAsync(existUser))
                {
                    var content = $@"Your account is locked out. To reset the password click this link: {user.ClientURI}";
                    //var message = new Message([user.Email!], "Locked out account information", content, null);
                    //await _emailSender.SendEmailAsync(message);

                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });
                }
            }

            if (await _userManager.GetTwoFactorEnabledAsync(existUser)) return await GenerateOTPFor2StepVerification(existUser);

            var token = await _jwtHandler.GenerateToken(existUser);
            await _userManager.ResetAccessFailedCountAsync(existUser);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        }

        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalAuth([FromBody] ExternalAuthDto externalAuth)
        {
            var payload = await _jwtHandler.VerifyGoogleToken(externalAuth);
            if (payload == null) return BadRequest("Invalid external Authentication");

            var info = new UserLoginInfo(externalAuth.Provider!, payload.Subject, externalAuth.Provider);
            var newUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (newUser == null)
            {
                newUser = await _userManager.FindByEmailAsync(payload.Email);
                if (newUser == null)
                {
                    newUser = new User { Email = payload.Email, UserName = payload.Email };
                    await _userManager.CreateAsync(newUser);
                    await _userManager.AddToRoleAsync(newUser, "Viewer");
                    await _userManager.AddLoginAsync(newUser, info);
                }

                else
                {
                    await _userManager.AddLoginAsync(newUser, info);
                }
            }

            if (newUser == null) return BadRequest("Invalid External Authentication");

            var token = await _jwtHandler.GenerateToken(newUser);

            return Ok(new AuthResponseDto { Token = token, IsAuthSuccessful = true });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            if (!ModelState.IsValid) return BadRequest();

            var existUser = await _userManager.FindByEmailAsync(forgotPassword.Email!);
            if (existUser == null) return NotFound("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(existUser);
            var param = new Dictionary<string, string>
            {
                {"token", token},
                {"email", forgotPassword.Email!}
            };

            var callback = QueryHelpers.AddQueryString(forgotPassword.ClientURI!, param!);
            //var message = new Message([existUser.Email!], "Reset password token", callback, null);
            //await _emailSender.SendEmailAsync(message);

            return Ok();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid) return BadRequest();

            var existUser = await _userManager.FindByEmailAsync(resetPassword.Email!);

            if (existUser == null) return NotFound("User not found");

            var resetPassResult = await _userManager.ResetPasswordAsync(existUser, resetPassword.Token!, resetPassword.Password!);

            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }

            await _userManager.SetLockoutEndDateAsync(existUser, new DateTime(2000, 1, 1));

            return Ok();
        }

        [HttpGet("EmailConfirm")]
        public async Task<IActionResult> EmailConfirm([FromQuery] string email, [FromQuery] string token)
        {
            var existUser = await _userManager.FindByEmailAsync(email);

            if (existUser == null) return NotFound("User not found");

            var confirmResult = await _userManager.ConfirmEmailAsync(existUser, token);

            if (!confirmResult.Succeeded) return BadRequest();

            return Ok();
        }

        [HttpPost("2StepVerification")]
        public async Task<IActionResult> TwoStepVerification([FromBody] TwoFactorDto twoFactor)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existUser = await _userManager.FindByEmailAsync(twoFactor.Email!);

            if (existUser == null) return NotFound("User not found");

            var validVerify = await _userManager.VerifyTwoFactorTokenAsync(existUser, twoFactor.Provider!, twoFactor.Token!);

            if (!validVerify) return BadRequest("Invalid token verification");

            var token = await _jwtHandler.GenerateToken(existUser);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        }

        private async Task<IActionResult> GenerateOTPFor2StepVerification(User user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

            if (!providers.Contains("Email")) return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid 2-Step Verification provider" });

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            //var message = new Message([user.Email!], "Auth token", token, null);
            //await _emailSender.SendEmailAsync(message);

            return Ok(new AuthResponseDto { Is2StepVerificationRequired = true, Provider = "Email" });
        }
    }
}