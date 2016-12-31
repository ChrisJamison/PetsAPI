using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using PetsAPI.Constants;
using PetsAPI.Models;

namespace PetsAPI.Helpers
{
    public static class ResponseHelper
    {
        public static HttpResponseMessage ErrorResult(HttpRequestMessage request, HttpStatusCode httpStatusCode, int errorCode, string errorDes, string errorTrace)
        {
            var errorModel = new ErrorModel
            {
                errorCode = errorCode,
                errorDes = errorDes
            };
            errorModel.errorMsg = ErrorMessages.GetLocalizedErrorMessage(errorModel.errorCode, "vi");
            errorModel.errorTrace = errorTrace;
            return request.CreateResponse(httpStatusCode, errorModel);
        }
    }
}