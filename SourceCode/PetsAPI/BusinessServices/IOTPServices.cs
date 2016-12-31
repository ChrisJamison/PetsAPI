using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace BusinessServices
{
    public interface IOTPServices
    {
        OTPEntity CreateOTP(OTPEntity otpEntity);
        IEnumerable<OTPEntity> GetOTPByUserAuthInfoId(int userAuthInfoId);
    }
}
