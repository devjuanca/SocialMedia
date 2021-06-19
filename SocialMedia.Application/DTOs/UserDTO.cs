using Microsoft.AspNetCore.Http;
using SocialMedia.Application.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.DTOs
{
   public class SMUserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        public string Lastname { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public string PhotoRoute { get; set; }
        public bool? Active { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }

    public class SMUserComandDTO : SMUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }

   



}
