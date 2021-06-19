using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.DTOs;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.Bussiness_Rules
{

    public interface ISMUserBussinessRules
    {
        void ApplyBussinessRules(SMUserComandDTO SMUser);

    }

    public class SMUserBussinessRules : ISMUserBussinessRules
    {
        readonly SocialMediaContext _ctx;
        readonly UserManager<SocialMedia.Domain.Entities.SMUser> _userManager;
        public SMUserBussinessRules(SocialMediaContext ctx, UserManager<SocialMedia.Domain.Entities.SMUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        public void ApplyBussinessRules(SMUserComandDTO SMUser)
        {
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

           

            if (SMUser.BirthDate.Value.Year > 1987)
            {
                errors.Add("Minimal Age Violation", new string[] { "It is required to be born before 1987." });
            }
            if (errors.Count > 0)
                throw new BussinessException("SMUser Bussiness Error", errors);
        }


    }
}
