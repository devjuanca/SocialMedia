using Microsoft.AspNetCore.Identity;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.Entities;
using SocialMedia.Domain.Entities;
using System.Threading.Tasks;

namespace SocialMedia.Application.Services
{
    public interface IChangePasswordService
    {
        Task ChangePassword(ChangePasswordEntity changePasswordEntity);
    }


    public class ChangePasswordService : IChangePasswordService
    {
        readonly UserManager<SMUser> _userManager;
        readonly IProcessIdentityErrors _errorsProcesing;
        public ChangePasswordService(UserManager<SMUser> userManager, IProcessIdentityErrors errorsProcesing)
        {
            _userManager = userManager;
            _errorsProcesing = errorsProcesing;
        }
        
        public async Task ChangePassword(ChangePasswordEntity changePasswordEntity)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(changePasswordEntity.UserName);

                var result = await _userManager.ChangePasswordAsync(user, changePasswordEntity.OldPassword, changePasswordEntity.NewPasword);
                if (!result.Succeeded)
                { throw new ApiExceptions("Change Password Error", _errorsProcesing.FromIdentityResultError(result.Errors)); }
            }
            catch (ApiExceptions ex)
            {
                throw ex;
            }

        }
    }
}