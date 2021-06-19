using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SocialMedia.Application.Entities;
using SocialMedia.Domain.Entities;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.Services
{
    public interface IConfirmEmail
    {
        Task<bool> ConfirmUserEmail(SMUser user);
    }


    public class ConfirmEmail : IConfirmEmail
    {
        readonly ISendEmail _emailService;
        readonly IUriService _uriService;
        readonly UserManager<SMUser> _userManager;
        public ConfirmEmail(ISendEmail emailService, IUriService uriService, UserManager<SMUser> userManager)
        {
            _userManager = userManager;
            _emailService = emailService;
            _uriService = uriService;
        }
        public async Task<bool> ConfirmUserEmail(SMUser user)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodingToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodingToken);

            UserToken userToken = new UserToken
            {
                UserId = user.Id,
                Token = validToken
            };

            MailRequest email = new MailRequest
            {
                Subject = "Confirmation Email",
                Body = $"Hello Mr. {user.Lastname} Please click in the link below to confirm your email address. " +
                $"{_uriService.GetIdentityTokenConfirmationUri(userToken, "/api/ConfirmAccount/")}",
                ToEmail = user.Email
            };

            return await _emailService.SendConfirmationEmail(email);

        }
    }

}
