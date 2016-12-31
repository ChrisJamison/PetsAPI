using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using AutoMapper;
using BusinessEntities;
using BusinessServices;
using MH.Utilities;
using PetsAPI.Constants;
using PetsAPI.Filters;
using PetsAPI.Helpers;
using PetsAPI.Models;

namespace PetsAPI.Controllers.Api
{
    [AuthorizationRequired]
    public class PetsController : ApiController
    {
        private readonly IPetServices _iPetServices;
        private readonly IImageServices _iImageServices;

        public PetsController(IPetServices iPetServices, IImageServices iImageServices)
        {
            _iPetServices = iPetServices;
            _iImageServices = iImageServices;
        }
        public HttpResponseMessage POST([FromBody]PetExpandedModel model)
        {
            try
            {
                // Upload Image
                var imageBinaryInBase64 = model.thumbNailInBase64;
                var avatarBinary = Convert.FromBase64String(imageBinaryInBase64);
                var stream = new MemoryStream();
                stream.Write(avatarBinary, 0, avatarBinary.Length);
                var image = Image.FromStream(stream);
                var filePath = HostingEnvironment.MapPath("~/Images/PetThumbnails");
                var fileName = Utils.NewGuid() + ".jpg";
                if (filePath != null)
                {
                    var fullPath = Path.Combine(filePath, fileName);
                    image.Save(fullPath);
                }

                var imageEntity = new ImageEntity()
                {
                    thumbnail = fileName
                };
                imageEntity = _iImageServices.CreateImage(imageEntity);

                // Create Pet
                model.imageId = imageEntity.id;
                var pet = Mapper.Map<PetExpandedModel, PetEntity>(model);
                pet = _iPetServices.CreatePet(pet);

                return Request.CreateResponse(HttpStatusCode.OK, pet);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
           
        }

        [Route("api/pets/{id}/getpetimage")]
        [HttpGet]
        public HttpResponseMessage GetPetImage(int id)
        {
            try
            {
                var pet = _iPetServices.GetPetById(id);
                if (pet == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_PET_NOT_EXISTED, ErrorCodeStrings.PET_IS_NOT_EXISTED, null);
                }
                var image = _iImageServices.GetImageById(pet.imageId);
                return Request.CreateResponse(HttpStatusCode.OK, image);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
           
        }
    }
}
