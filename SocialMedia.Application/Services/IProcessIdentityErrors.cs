using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SocialMedia.Application.Services
{
    public interface IProcessIdentityErrors
    {
        Dictionary<string, string[]> FromIdentityResultError(IEnumerable<IdentityError> identityErrors);
    }

    public class ProcessIdentityErrors : IProcessIdentityErrors
    {
        public Dictionary<string, string[]> FromIdentityResultError(IEnumerable<IdentityError> identityErrors)
        {
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
            List<string> iErrors = new List<string>();
            foreach (var item in identityErrors)
            {
                iErrors.Add(item.Description);
            }
            errors.Add("User Creating Errors", iErrors.ToArray());

            return errors;
        }
    }
}