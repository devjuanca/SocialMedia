using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.Entities;
using SocialMedia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.Services
{
    public interface IAccountConfirmed
    {
        Task Confirm(UserToken userToken);
    }

    public class AccountConfirmed : IAccountConfirmed
    {
        readonly UserManager<SMUser> _userManager;
        readonly IProcessIdentityErrors _processIdentityErrors;

        public AccountConfirmed(UserManager<SMUser> userManager, IProcessIdentityErrors processIdentityErrors)
        {
            _userManager = userManager;
            _processIdentityErrors = processIdentityErrors;
        }
        public async Task Confirm(UserToken userToken)
        {

            try
            {
                var user = await _userManager.FindByIdAsync(userToken.UserId);
                var decode_token = WebEncoders.Base64UrlDecode(userToken.Token);
                var returnUrl = userToken.ReturnUrl;
                var validToken = Encoding.UTF8.GetString(decode_token);

                var result = await _userManager.ConfirmEmailAsync(user, validToken);
                if (!result.Succeeded)
                {
                    throw new ApiExceptions("Email confirmation error", _processIdentityErrors.FromIdentityResultError(result.Errors));
                }
            }
            catch (ApiExceptions ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new ApiExceptions("Email confirmation error", new Dictionary<string, string[]>() { { " ", new string[] { "Some errores occured." } } });
            }
        }

    }
}
