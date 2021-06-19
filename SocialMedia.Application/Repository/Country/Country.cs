using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.Repository.Country
{
    public interface ICountry
    {
        Task<IEnumerable<CountryDTO>> GetCountries();
    }

    public class Country : ICountry
    {
        readonly SocialMediaContext _ctx;
        public Country(SocialMediaContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<CountryDTO>> GetCountries()
        {
            return await _ctx.Country.AsNoTracking().Select(a => new CountryDTO
            {
                Id = a.CountryId,
                Name = a.Name
            }).ToListAsync();
        }
    }
}
