using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Domain.Entities;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Application.Services
{
    public interface IUploadImages
    {
       Task<string> UploadProfilePhoto(byte[] image, string id);
    }


    public class UploadImages : IUploadImages
    {
        readonly IHostingEnvironment _hosting;
        readonly UserManager<SMUser> _userManager;
        IConfiguration _configuration;
        private string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };
        public UploadImages(IHostingEnvironment hosting, UserManager<SMUser> userManager, IConfiguration configuration)
        {
            _hosting = hosting;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> UploadProfilePhoto(byte[] image, string username)
        {
            try
            {

               
                //await Task.Run(() => { 
               return await Task<string>.Run(() =>
                { 
                    string imageName = string.Empty;

                    string root = $"{_hosting.ContentRootPath}\\MystaticFiles";
                    DirectoryInfo userDirectory;
                    DirectoryInfo imageDirectory = new DirectoryInfo(root + "/Images");

                    if (imageDirectory.GetDirectories(username).Length == 0)
                        userDirectory = imageDirectory.CreateSubdirectory(username);
                    else
                        userDirectory = imageDirectory.GetDirectories(username)[0];
                    Image temp = null;
                    using (var ms = new MemoryStream(image))
                    {
                        temp = Image.FromStream(ms);


                        imageName = Guid.NewGuid() + ".jpeg";
                        string imageRoute = Path.Combine(userDirectory.FullName, imageName);



                        if (ms.Length > Convert.ToInt32(_configuration["FileSizeLimit"]))
                        {
                            throw new ImageUploadException("File must be less than 2 mb.", null);
                        }
                        //if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                        //{
                        //    throw new ImageUploadException("Invalid File. Only images allowed.", null);
                        //}

                        temp.Save(imageRoute, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //});
                    }
               
                return imageName; 
                });
            }
            catch (Exception ex)
            {
                throw new ImageUploadException(ex.Message, null);
            }
        }

    }
}