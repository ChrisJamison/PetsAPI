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
using PetsAPI.AppConfig;
using PetsAPI.Constants;
using PetsAPI.Filters;
using PetsAPI.Helpers;
using PetsAPI.Models;

namespace PetsAPI.Controllers.Api
{
    public class UserAuthInfosController : ApiController
    {
        private readonly IUserServices _iUserServices;
        private readonly IUserAuthInfoServices _iUserAuthInfoServices;
        private readonly IOTPServices _iOtpServices;
        private readonly IPetServices _iPetServices;
        private readonly IArticleServices _iArticleServices;
        private readonly IImageServices _iImageServices;
        public UserAuthInfosController(IUserServices iUserServices, IUserAuthInfoServices iUserAuthInfoServices, IOTPServices iOtpServices, IPetServices iPetServices, IArticleServices iArticleServices, IImageServices iImageServices)
        {
            _iUserServices = iUserServices;
            _iUserAuthInfoServices = iUserAuthInfoServices;
            _iOtpServices = iOtpServices;
            _iPetServices = iPetServices;
            _iArticleServices = iArticleServices;
            _iImageServices = iImageServices;
        }

        public HttpResponseMessage Post([FromBody]UserModel model)
        {
            try
            {
                var user = _iUserServices.GetUserByInfo(model.email);
                if (user != null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_EMAIL_IS_ALREADY_EXISTED, ErrorCodeStrings.EMAIL_IS_ALREADY_EXISTED,
                        null);
                }
                    user = _iUserServices.GetUserByInfo(model.phone);
                if (user != null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_PHONE_IS_ALREADY_EXISTED, ErrorCodeStrings.PHONE_IS_ALREADY_EXISTED,
                        null);
                }
                var userEntity = Mapper.Map<UserModel, UserEntity>(model);
                userEntity = _iUserServices.CreateUser(userEntity);
                var userAuthInfoEntity = new UserAuthInfoEntity()
                {
                    userId = userEntity.id,
                    passwordHash = model.passwordHash 
                };
                userAuthInfoEntity = _iUserAuthInfoServices.CreateUserAuthInfo(userAuthInfoEntity);
                return Request.CreateResponse(HttpStatusCode.OK, new {userEntity, userAuthInfoEntity});
            }
            catch (Exception e)
            {
               return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [Route("api/userauthinfos/requestotp")]
        [HttpPost]
        public HttpResponseMessage RequestOTP([FromBody] UserModel model)
        {
            try
            {
                var user = _iUserServices.GetUserByInfo(model.email);
                if (user == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_USER_NOT_EXISTED, ErrorCodeStrings.USER_IS_NOT_EXISTED, null);
                }

                var userAuthInfo = _iUserAuthInfoServices.GetUserAuthInfoByUserId(user.id);
                if (userAuthInfo == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_USER_AUTH_INFO_NOT_EXISTED, ErrorCodeStrings.USER_IS_NOT_REGISTERED, null);
                }
                
                // Generate OTP
                var otpCode = Utils.OTPNumber();

                // Send OTP Email
                //string link = string.Format(Request.Url.Host + "/satraservices/api/confirm?session={0}&otp={1}", sessionKey, OTPKey);
                string mailbody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Template/MailTemplate.html"));
                mailbody = mailbody.Replace("--OTP--", otpCode);
                mailbody = mailbody.Replace("source-path", "http://210.211.118.178/PetsAPI/Template/background-4.jpg");
                Utils.GoogleMail(user.email, ConfigKey.MAILTITLE, mailbody, ConfigKey.EMAIL, ConfigKey.PASSWORD);

                // Send OTP SMS

                var otp = new OTPEntity()
                {
                    otpCode = otpCode,
                    userAuthInfoId = userAuthInfo.id,
                    expiredOn = DateTime.Now.AddMinutes(UserAppConstant.OTP_CODE_EXPIRED_ON)
                };

                _iOtpServices.CreateOTP(otp);
                const string data = "Send OTP Successfully";
                return Request.CreateResponse(HttpStatusCode.OK, new { data });
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [Route("api/userauthinfos/resetpassword")]
        public HttpResponseMessage ResetPassword([FromBody] UserModel model)
        {
            try
            {
                var user = _iUserServices.GetUserByInfo(model.email);
                if (user == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_USER_NOT_EXISTED, ErrorCodeStrings.USER_IS_NOT_EXISTED, null);
                }

                var userAuthInfo = _iUserAuthInfoServices.GetUserAuthInfoByUserId(user.id);
                if (userAuthInfo == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_USER_AUTH_INFO_NOT_EXISTED, ErrorCodeStrings.USER_IS_NOT_REGISTERED, null);
                }

                var otp = _iOtpServices.GetOTPByUserAuthInfoId(userAuthInfo.id);
                if (otp == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_OTP_NOT_EXISTED, ErrorCodeStrings.OTP_IS_NOT_EXISTED, null);
                }

                if (otp.All(c => c.otpCode != model.oTPCode || c.expiredOn < DateTime.Now))
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_INVALID_OTP, ErrorCodeStrings.INCORRECT_OTP, null);
                }
                userAuthInfo.passwordHash = model.passwordHash;
                userAuthInfo = _iUserAuthInfoServices.UpdateUserAuthInfoEntity(userAuthInfo.id, userAuthInfo);

                return Request.CreateResponse(HttpStatusCode.OK, new {user, userAuthInfo});
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        public HttpResponseMessage Get(int id)
        {
            try
            {
                var userAuthInfo = _iUserAuthInfoServices.GetUserAuthInforById(id);
                if (userAuthInfo == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_USER_AUTH_INFO_NOT_EXISTED, ErrorCodeStrings.USER_IS_NOT_REGISTERED, null);
                }
                var user = _iUserServices.GetUserById(userAuthInfo.userId);
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [AuthorizationRequired]
        [Route("api/userauthinfos/{id}/updateavatar")]
        [HttpPost]
        public HttpResponseMessage UpdateAvatarThumbnail(int id,[FromBody] UserModel model)
        {
            try
            {
                var userAuthInfo = _iUserAuthInfoServices.GetUserAuthInforById(id);
                if (userAuthInfo == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_USER_AUTH_INFO_NOT_EXISTED, ErrorCodeStrings.USER_IS_NOT_REGISTERED, null);
                }
                var user = _iUserServices.GetUserById(userAuthInfo.userId);
                if (user == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_USER_NOT_EXISTED, ErrorCodeStrings.USER_IS_NOT_EXISTED, null);
                }

                // Upload Image
                var imageBinaryInBase64 = model.avatarThumbnailInBase64;
                var avatarBinary = Convert.FromBase64String(imageBinaryInBase64);
                var stream = new MemoryStream();
                stream.Write(avatarBinary, 0, avatarBinary.Length);
                var image = Image.FromStream(stream);
                var filePath = HostingEnvironment.MapPath("~/Images/UserThumbnails");
                var fileName = Utils.NewGuid() + ".jpg";
                if (filePath != null)
                {
                    var fullPath = Path.Combine(filePath, fileName);
                    image.Save(fullPath);
                }
                user.avatarThumbnail = fileName;
                _iUserServices.UpdateAvatarThumbnail(user);
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [AuthorizationRequired]
        [Route("api/userauthinfos/{id}/updatecover")]
        [HttpPost]
        public HttpResponseMessage UpdateCoverThumbnail(int id, [FromBody] UserModel model)
        {
            try
            {
                var userAuthInfo = _iUserAuthInfoServices.GetUserAuthInforById(id);
                if (userAuthInfo == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_USER_AUTH_INFO_NOT_EXISTED, ErrorCodeStrings.USER_IS_NOT_REGISTERED, null);
                }
                var user = _iUserServices.GetUserById(userAuthInfo.userId);
                if (user == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_USER_NOT_EXISTED, ErrorCodeStrings.USER_IS_NOT_EXISTED, null);
                }

                // Upload Image
                var imageBinaryInBase64 = model.coverThumbnailInBase64;
                var avatarBinary = Convert.FromBase64String(imageBinaryInBase64);
                var stream = new MemoryStream();
                stream.Write(avatarBinary, 0, avatarBinary.Length);
                var image = Image.FromStream(stream);
                var filePath = HostingEnvironment.MapPath("~/Images/UserThumbnails");
                var fileName = Utils.NewGuid() + ".jpg";
                if (filePath != null)
                {
                    var fullPath = Path.Combine(filePath, fileName);
                    image.Save(fullPath);
                }
                user.coverThumbnail = fileName;
                _iUserServices.UpdateCoverThumbnail(user);
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [AuthorizationRequired]
        [Route("api/userauthinfos/{id}/articles")]
        [HttpGet]
        public HttpResponseMessage GetArticle(int id)
        {
            try
            {
                var pets = _iPetServices.GetPetsByUserAuthInfoId(id);
                if (pets == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, (IEnumerable<PetEntity>) null);
                }
                var petEntities = pets as IList<PetEntity> ?? pets.ToList();
                var articlesResult = petEntities.Select(pet => _iArticleServices.GetArticleByPetId(pet.id)).ToList();
                var images = petEntities.Select(pet => _iImageServices.GetImageById(pet.imageId)).ToList();

                var articles = Mapper.Map<List<ArticleEntity>, List<ArticleExpandedModel>>(articlesResult);
                foreach (var article in articles)
                {
                    if (article == null) continue;
                    foreach (var pet in petEntities.Where(pet => article.petId == pet.id))
                    {
                        article.Pet = Mapper.Map<PetEntity, PetModel>(pet);
                        var userAuthInfor = _iUserAuthInfoServices.GetUserAuthInforById(pet.userAuthInfoId);
                        article.Pet.User = _iUserServices.GetUserById(userAuthInfor.userId);
                        foreach (var image in images.Where(image => article.Pet.imageId == image.id))
                        {
                            article.Pet.Image = image;
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, articles);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [AuthorizationRequired]
        public HttpResponseMessage Put(int id, [FromBody]UserEntity model)
        {
            try
            {
                var user = _iUserServices.UpdateUser(id, model);
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [AuthorizationRequired]
        [Route("api/userauthinfos/{id}/petimages")]
        [HttpGet]
        public HttpResponseMessage GetPetImages(int id)
        {
            try
            {
                var pets = _iPetServices.GetPetsByUserAuthInfoId(id);
                if (pets == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, (IEnumerable<PetEntity>)null);
                }

                var result = Mapper.Map<List<PetEntity>, List<PetModel>>(pets.ToList());
                foreach (var pet in result)
                {
                    pet.Image = _iImageServices.GetImageById(pet.imageId);
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }  
        }
    }
}