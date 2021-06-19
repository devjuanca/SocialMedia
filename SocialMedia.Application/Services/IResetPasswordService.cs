using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using PasswordGenerator;
using SocialMedia.Application.Entities;
using SocialMedia.Domain.Entities;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.Services
{
    public interface IResetPasswordService
    {
        Task SendTokenToUser(string userName);
         Task ResetPassword(UserToken userToken);
    }

    public class ResetPasswordService : IResetPasswordService
    {
        readonly UserManager<SMUser> _userManaager;
        readonly ISendEmail _sendEmail;
        readonly IUriService _uriService;
        public ResetPasswordService(UserManager<SMUser> userManaager, ISendEmail sendEmail, IUriService uriService)
        {
            _userManaager = userManaager;
            _sendEmail = sendEmail;
            _uriService = uriService;
        }

        public async Task SendTokenToUser(string userName)
        {
            var user = await _userManaager.FindByNameAsync(userName);
            var token = await _userManaager.GeneratePasswordResetTokenAsync(user);


            var encodingToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodingToken);

            var userToken = new UserToken { UserId = user.Id, Token = validToken };
            await _sendEmail.SendConfirmationEmail(new MailRequest
            {
                Subject = "Reset Password Request",
                Body = $"Hi, you have requested to reset your password. Please click the following link " +
                 $"{_uriService.GetIdentityTokenConfirmationUri(userToken, "/api/AccountManagement/PerformResetPassword/")}",
                ToEmail = user.Email
            });

        }

        public async Task ResetPassword(UserToken userToken)
        {
            try
            {
                var user = await _userManaager.FindByIdAsync(userToken.UserId);
                var pwd = new Password();
                pwd.IncludeLowercase();
                pwd.IncludeNumeric();
                pwd.IncludeUppercase();
                pwd.IncludeSpecial();
                pwd.LengthRequired(6);
                string password = pwd.Next();

                //var encodingPassword = Encoding.UTF8.GetBytes(password);
                //var validPassword = WebEncoders.Base64UrlEncode(encodingPassword);

                var decode_token = WebEncoders.Base64UrlDecode(userToken.Token);
                var validToken = Encoding.UTF8.GetString(decode_token);


                IdentityResult result = await _userManaager.ResetPasswordAsync(user, validToken, password);

                if (!result.Succeeded)
                {
                    throw new Exception();
                }
                await _sendEmail.SendConfirmationEmail(new MailRequest
                {
                    Subject = "New Password",
                    Body = $"You have reset your password, the new one is {password}",
                    ToEmail = user.Email
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}