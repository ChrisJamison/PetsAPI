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
    public class UserAuthInfoServices : IUserAuthInfoServices
    {
        private readonly UnitOfWork _unitOfWork;
        public UserAuthInfoServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public UserAuthInfoEntity CreateUserAuthInfo(UserAuthInfoEntity userAuthInfoEntity)
        {
            if (userAuthInfoEntity == null) return null;
            using (var scope = new TransactionScope())
            {
                var userAuthInfo = Mapper.Map<UserAuthInfoEntity, UserAuthInfo>(userAuthInfoEntity);
                _unitOfWork.UserAuthInfoRepository.Insert(userAuthInfo);
                _unitOfWork.Save();
                scope.Complete();
                userAuthInfoEntity.id = userAuthInfo.Id;
                return userAuthInfoEntity;
            }
        }


        public UserAuthInfoEntity GetUserAuthInfoByUserId(int id)
        {
            var userAuthInfo = _unitOfWork.UserAuthInfoRepository.Get(p => p.UserId == id);
            if (userAuthInfo == null) return null;
            var model = Mapper.Map<UserAuthInfo, UserAuthInfoEntity>(userAuthInfo);
            return model;
        }


        public UserAuthInfoEntity UpdateUserAuthInfoEntity(int id, UserAuthInfoEntity userAuthInfoEntity)
        {
            var userAuthInfo = _unitOfWork.UserAuthInfoRepository.GetById(id);
            if (userAuthInfo == null)
                return null;
            userAuthInfo.PasswordHash = userAuthInfoEntity.passwordHash;
            userAuthInfo.UserId = userAuthInfoEntity.userId;
            using (var scope = new TransactionScope())
            {
                _unitOfWork.UserAuthInfoRepository.Update(userAuthInfo);
                _unitOfWork.Save();
                scope.Complete();
                return userAuthInfoEntity;
            }
        }


        public UserAuthInfoEntity GetUserAuthInforById(int id)
        {
            var userAuthInfo = _unitOfWork.UserAuthInfoRepository.GetById(id);
            if (userAuthInfo == null)
                return null;
            var userAuthInfoEntity = Mapper.Map<UserAuthInfo, UserAuthInfoEntity>(userAuthInfo);
            return userAuthInfoEntity;
        }
    }
}
