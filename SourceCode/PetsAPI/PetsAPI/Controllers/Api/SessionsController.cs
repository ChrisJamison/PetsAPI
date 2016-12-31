using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using BusinessServices;
using MH.Utilities;
using PetsAPI.Constants;
using PetsAPI.Helpers;
using PetsAPI.Models;

namespace PetsAPI.Controllers.Api
{
    public class SessionsController : ApiController
    {
        private readonly ISessionServices _iSessionServices;
        private readonly IUserAuthInfoServices _iUserAuthInfoServices;
        private readonly IUserServices _iUserServices;

        public SessionsController(ISessionServices iSessionServices, IUserAuthInfoServices iUserAuthInfoServices, IUserServices iUserServices)
        {
            _iSessionServices = iSessionServices;
            _iUserAuthInfoServices = iUserAuthInfoServices;
            _iUserServices = iUserServices;
        }

        [Route("api/sessions/verify")]
        public HttpResponseMessage Post([FromBody]UserModel model)
        {
            try
            {
                var user = _iUserServices.GetUserByInfo(model.info);
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
                if (userAuthInfo.passwordHash != model.passwordHash)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_INVALID_PASSWORD, ErrorCodeStrings.INVALID_PASSWORD, null);
                }

                var authToken = Utils.NewGuid();
                var sessionEntity = new SessionEntity
                {
                    authToken = Utils.MD5Hash(authToken),
                    userAuthInfoId = userAuthInfo.id,
                    isVerified = true,
                    expiredOn = DateTime.Now.AddMonths(1)
                };
                var session = _iSessionServices.CreateSession(sessionEntity);
                return Request.CreateResponse(HttpStatusCode.OK, session);
            }

            catch (Exception e)
            {

                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }
    }
}