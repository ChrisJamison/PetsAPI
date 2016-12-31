using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;

namespace BusinessServices
{
    public class OTPServices : IOTPServices
    {
        private readonly UnitOfWork _unitOfWork;

        public OTPServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BusinessEntities.OTPEntity CreateOTP(BusinessEntities.OTPEntity otpEntity)
        {
            var otp = Mapper.Map<OTPEntity, OTP>(otpEntity);
            using (var scope = new TransactionScope())
            {
                _unitOfWork.OtpRepository.Insert(otp);
                _unitOfWork.Save();
                scope.Complete();
                otpEntity.id = otp.Id;
                return otpEntity;
            }
        }


        public IEnumerable<OTPEntity> GetOTPByUserAuthInfoId(int userAuthInfoId)
        {
            var otp = _unitOfWork.OtpRepository.GetMany(c => c.UserAuthInfoId == userAuthInfoId).ToList();
            if (!otp.Any())
            {
                return null;
            }
            var otpEntity = Mapper.Map<List<OTP>, List<OTPEntity>>(otp);
            return otpEntity;
        }
    }
}
