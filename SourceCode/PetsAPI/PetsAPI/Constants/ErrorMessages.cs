using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsAPI.Constants
{
    public class ErrorMessages
    {
        private static Dictionary<int, string> GetMapForLanguage(String languageCode)
        {
            if (languageCode.Equals("vi"))
            {
                return SMessageMapForVi;
            }
            return languageCode.Equals("en") ? SMessageMapForEn : SMessageMapForVi;
        }
        public static Dictionary<int, string> SMessageMapForVi = new Dictionary<int, string>();
        public static Dictionary<int, string> SMessageMapForEn = new Dictionary<int, string>();

        public const int ERROR_MSG_EMAIL_IS_ALREADY_EXISTED = 60;
        public const int ERROR_MSG_PHONE_IS_ALREADY_EXISTED = 61;
        public const int ERROR_MSG_USER_NOT_EXISTED= 70;
        public const int ERROR_MSG_USER_AUTH_INFO_NOT_EXISTED = 71;
        public const int ERROR_MSG_PET_NOT_EXISTED = 72;
        public const int ERROR_MSG_OTP_NOT_EXISTED = 73;
        public const int ERROR_MSG_INVALID_PASSWORD = 81;
        public const int ERROR_MSG_INVALID_OTP = 82;
        public const int ERROR_MSG_SERVER_ERROR = 500;

        static ErrorMessages()
        {
            SMessageMapForVi.Add(ERROR_MSG_USER_NOT_EXISTED, "Tài khoản này không tồn tại, vui lòng gọi tổng đài để cập nhật số phone");
            SMessageMapForEn.Add(ERROR_MSG_USER_NOT_EXISTED, "This account does not exist, please contact with manager to update phone number");
            SMessageMapForVi.Add(ERROR_MSG_USER_AUTH_INFO_NOT_EXISTED, "Tài khoản này chưa đăng ký, vui lòng đăng ký để sử dụng dịch vụ");
            SMessageMapForEn.Add(ERROR_MSG_USER_AUTH_INFO_NOT_EXISTED, "This account does not registered, please register to use our service");
            SMessageMapForVi.Add(ERROR_MSG_SERVER_ERROR, "Server đang gặp lỗi, vui lòng đợi cập nhật");
            SMessageMapForEn.Add(ERROR_MSG_SERVER_ERROR, "Internal Server ERROR, please wait for update");
            SMessageMapForVi.Add(ERROR_MSG_INVALID_PASSWORD, "Mật khẩu không chính xác");
            SMessageMapForEn.Add(ERROR_MSG_INVALID_PASSWORD, "Incorrect password");
            SMessageMapForVi.Add(ERROR_MSG_PET_NOT_EXISTED, "Thú cưng không tồn tại");
            SMessageMapForEn.Add(ERROR_MSG_PET_NOT_EXISTED, "Pet is not existed");
            SMessageMapForVi.Add(ERROR_MSG_OTP_NOT_EXISTED, "OTP không tồn tại");
            SMessageMapForEn.Add(ERROR_MSG_OTP_NOT_EXISTED, "OTP is not existed");
            SMessageMapForVi.Add(ERROR_MSG_INVALID_OTP, "Mã OTP không chính xác");
            SMessageMapForEn.Add(ERROR_MSG_INVALID_OTP, "Incorrect OTP code");
            SMessageMapForVi.Add(ERROR_MSG_EMAIL_IS_ALREADY_EXISTED, "Email đã tồn tại");
            SMessageMapForEn.Add(ERROR_MSG_EMAIL_IS_ALREADY_EXISTED, "Email is already existed");
            SMessageMapForVi.Add(ERROR_MSG_PHONE_IS_ALREADY_EXISTED, "Phone đã tồn tại");
            SMessageMapForEn.Add(ERROR_MSG_PHONE_IS_ALREADY_EXISTED, "Phone is already existed");
        }

        public static string GetLocalizedErrorMessage(int errorMsgCode, String languageCode)
        {
            return GetMapForLanguage(languageCode)[errorMsgCode];
        }
    }
}