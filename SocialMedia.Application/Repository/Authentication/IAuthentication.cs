using Microsoft.AspNetCore.Identity;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.Entities;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.Repository.Authentication
{
    public interface IAuthentication
    {
        Task<SocialMedia.Domain.Entities.SMUser> Login(UserLogin userLogin);
    }

    public class Authentication : IAuthentication
    {
        
        readonly UserManager<SocialMedia.Domain.Entities.SMUser> _userManager;
        readonly SignInManager<SocialMedia.Domain.Entities.SMUser> _signInManager;
        readonly Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
        public Authentication(UserManager<SocialMedia.Domain.Entities.SMUser> userManager, SignInManager<SocialMedia.Domain.Entities.SMUser> signInManager)
        {
        
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SocialMedia.Domain.Entities.SMUser> Login(UserLogin userLogin)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userLogin.UserName);

                if (user == null)
                {
                    errors.Add("User Not Found", new[] { "The user does' t exist." });
                    throw new AuthenticationException("The user does't exist.", errors);
                }
                var signingResult = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);
                if (!signingResult.Succeeded)
                {
                    errors.Add("Wrong Password", new[] { "The password is incorrect, At 6 wrong attemps you will be blocked." });
                    throw new AuthenticationException("Wrong Password. ", errors);
                }

                return user;
            }
            catch (AuthenticationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                errors.Add("ApiError", new string[] { ex.Message });
                throw new ApiExceptions("Api Exception", errors);
            }
        }
    }
}
