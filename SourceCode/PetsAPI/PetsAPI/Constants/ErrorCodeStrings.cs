using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsAPI.Constants
{
    public class ErrorCodeStrings
    {
        public const string USER_IS_NOT_EXISTED = "User is not existed";
        public const string SERVER_ERROR = "Internal error server";
        public const string USER_IS_NOT_REGISTERED = "User is not registered";
        public const string INVALID_PASSWORD = "Incorrect password";
        public const string PET_IS_NOT_EXISTED = "Pet is not existed";
        public const string OTP_IS_NOT_EXISTED = "OTP is not existed";
        public const string INCORRECT_OTP = "Incorrect OTP";
        public const string EMAIL_IS_ALREADY_EXISTED = "Email is already existed";
        public const string PHONE_IS_ALREADY_EXISTED = "Phone is already existed";
    }
}