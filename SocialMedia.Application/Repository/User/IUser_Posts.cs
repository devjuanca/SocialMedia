using Microsoft.AspNetCore.Identity;
using SocialMedia.Application.Bussiness_Rules;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Services;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SocialMedia.Application.Repository.SMUser.Posts
{
    public interface ISMUser_Posts
    {
        Task<SMUserComandDTO> NewSMUser(SMUserComandDTO SMUserDTO);
        Task UpdateSMUser(SMUserComandDTO SMUserDTO);
        Task DeleteSMUser(string post_id);
        Task DisableSMUser(string id);
        Task EnableSMUser(string id);
        
    }


    public class SMUser_Posts : ISMUser_Posts
    {
        readonly SocialMediaContext _ctx;
        readonly ISMUserBussinessRules _SMUserBussinessRules;
        readonly IConfirmEmail _confirmEmail;
        readonly IUploadImages _uploadImages;
        readonly UserManager<SocialMedia.Domain.Entities.SMUser> _userManager;
        readonly Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
        public SMUser_Posts(SocialMediaContext ctx, IConfirmEmail confirmEmail, ISMUserBussinessRules SMUserBussinessRules, UserManager<SocialMedia.Domain.Entities.SMUser> userManager, IUploadImages uploadImages)
        {
            _ctx = ctx;
            _userManager = userManager;
            _SMUserBussinessRules = SMUserBussinessRules;
            _confirmEmail = confirmEmail;
            _uploadImages = uploadImages;
        }

        public async Task<SMUserComandDTO> NewSMUser(SMUserComandDTO userDTO)
        {
            try
            {
                //Aplico las reglas de negocio.
                _SMUserBussinessRules.ApplyBussinessRules(userDTO);

                string image_name = null;
                //if (userDTO.Photo != null)
                //    image_name = await _uploadImages.UploadProfilePhoto(userDTO.Photo, userDTO.UserName);
                //Subo la imagen y retorno el nombre con que se guardó en BD.

                if(userDTO.ProfilePhoto != null)
                    image_name = await _uploadImages.UploadProfilePhoto(userDTO.ProfilePhoto, userDTO.UserName);

                SocialMedia.Domain.Entities.SMUser user = new SocialMedia.Domain.Entities.SMUser
                {
                    Name = userDTO.Name,
                    ProfilePhoto = image_name,
                    Lastname = userDTO.Lastname,
                    BirthDate = userDTO.BirthDate,
                    Active = true,
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    PhoneNumber = userDTO.Phone,
                    CountryId = userDTO.CountryId
                };
                //Creo el objeto SMuser.

                IdentityResult result = await _userManager.CreateAsync(user, userDTO.Password);
                //Inserto el nuevo usuario y obtengo un resultado.

                if(result.Succeeded)
                    await _confirmEmail.ConfirmUserEmail(user, userDTO.ReturnUrl);
                //si el resultado es exitoso envio confirmación por email.

                if (!result.Succeeded)
                {
                    throw new ApiExceptions("An error ocured when creating a new user", FromIdentityResultError(result.Errors));
                }
                //Si hubo un error en el envio de la confirmación arrojo una excepción

                return new SMUserComandDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Lastname = user.Lastname,
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Active = user.Active,
                    UserName = user.UserName,
                    //ProfilePhoto = user.ProfilePhoto
                    
                };
            }
            catch (BussinessException e)
            {
                throw e; //En caso de excepciones de negocio se disparan aqui.
            }
            catch (ImageUploadException e)
            {
                throw e; //En caso de excepciones al subir la imagen.
            }
            catch (Exception e)
            {
                if (e is ApiExceptions)
                {
                    throw new ApiExceptions((e as ApiExceptions).Message, (e as ApiExceptions).Errors);
                }// Si la excepcion fue disparada como un ApiExceptions la atrapa y dispara con los errores en un diccionario.
                else
                {
                    errors.Add("ApiError", new string[] { e.Message });
                    throw new ApiExceptions(e.Message, errors);
                    // Si es otro tipo de excepcion se atrapa y se dispara como ApiException.
                }

                //En este caso falta implentar eliminar la imagen que se subió al servidor
            }
        }

        public async Task DeleteSMUser(string SMUser_id)
        {
            try
            {
                var SMUser = await _ctx.Users.FindAsync(SMUser_id); //Busco al usuario en tabla SMUser.
                if (SMUser == null) //De no existir disparo un NotFoundException.
                {
                    errors.Add("Not Found", new string[] { "SMUsers Not Found" });
                    throw new NotFoundException("SMUsers Not Found", errors);
                }

                IdentityResult result = await _userManager.DeleteAsync(SMUser);

                if (!result.Succeeded)
                {
                    //En caso de ocurrir un error disparo una excepcion.
                    throw new ApiExceptions("An error ocured when creating a new user", FromIdentityResultError(result.Errors));
                }

            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {

                if (e is ApiExceptions)
                {
                    throw new ApiExceptions((e as ApiExceptions).Message, (e as ApiExceptions).Errors);
                }// Si la excepcion fue disparada como un ApiExceptions la atrapa y dispara con los errores en un diccionario.
                else
                {
                    errors.Add("ApiError", new string[] { e.Message });
                    throw new ApiExceptions(e.Message, errors);
                    // Si es otro tipo de excepcion se atrapa y se dispara como ApiException.
                }
            }

        }

        public async Task UpdateSMUser(SMUserComandDTO userDto)
        {
            try
            {
                 _SMUserBussinessRules.ApplyBussinessRules(userDto);
                //Aplico las reglas de negocio.

                var SMUser = await _userManager.FindByIdAsync(userDto.Id);
                if (SMUser == null)
                {
                    errors.Add("Not Found", new string[] { "SMUsers Not Found" });
                    throw new NotFoundException("SMUsers Not Found", errors);
                }
                //Busco al usuario, en caso de no encontrarlo arrojo una excepcion.

                string image_name = null;
               

                if (userDto.ProfilePhoto != null)
                    image_name = await _uploadImages.UploadProfilePhoto(userDto.ProfilePhoto, userDto.UserName);

                SMUser.Name = userDto.Name;
                SMUser.Lastname = userDto.Lastname;
                SMUser.BirthDate = userDto.BirthDate;
                SMUser.Email = userDto.Email;
                SMUser.PhoneNumber = userDto.Phone;
                if(image_name!=null)
                    SMUser.ProfilePhoto = image_name;
                SMUser.CountryId = userDto.CountryId;
                //modifico los datos

               
                

                await _ctx.SaveChangesAsync();
                //Guardo los cambios
            }
            catch (BussinessException e)
            {
                throw e; //En caso de excepciones de negocio se disparan aqui.
            }
            catch (ImageUploadException e)
            {
                throw e; //En caso de excepciones al subir la imagen.
            }
            catch (Exception e)
            {
                if (e is ApiExceptions)
                {
                    throw new ApiExceptions((e as ApiExceptions).Message, (e as ApiExceptions).Errors);
                }// Si la excepcion fue disparada como un ApiExceptions la atrapa y dispara con los errores en un diccionario.
                else
                {
                    errors.Add("ApiError", new string[] { e.Message });
                    throw new ApiExceptions(e.Message, errors);
                    // Si es otro tipo de excepcion se atrapa y se dispara como ApiException.
                }

                //En este caso falta implentar eliminar la imagen que se subió al servidor
            }

        }

        public async Task DisableSMUser(string id)
        {
            try
            {
                var SMUser = await _userManager.FindByIdAsync(id);
                if (SMUser == null)
                {
                    errors.Add("Not Found", new string[] { "SMUsers Not Found" });
                    throw new NotFoundException("SMUsers Not Found", errors);
                }
                SMUser.Active = false;
                await _ctx.SaveChangesAsync();
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions("Api Exception", errors);
            }
        }

        public async Task EnableSMUser(string id)
        {
            try
            {
                var SMUser = await _userManager.FindByIdAsync(id);
                if (SMUser == null)
                {
                    errors.Add("Not Found", new string[] { "SMUsers Not Found" });
                    throw new NotFoundException("SMUsers Not Found", errors);
                }
                SMUser.Active = true;
                await _ctx.SaveChangesAsync();
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions("Api Exception", errors);
            }
        }

        private Dictionary<string, string[]> FromIdentityResultError(IEnumerable<IdentityError> identityErrors)
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
