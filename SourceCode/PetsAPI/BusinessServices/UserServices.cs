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
    public class UserServices : IUserServices
    {
        private readonly UnitOfWork _unitOfWork;
        public UserServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BusinessEntities.UserEntity CreateUser(BusinessEntities.UserEntity userEntity)
        {
            if (userEntity == null) return null;
            using (var scope = new TransactionScope())
            {
                var user = Mapper.Map<UserEntity, User>(userEntity);
                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();
                scope.Complete();
                userEntity.id = user.Id;
                return userEntity;
            }

        }


        public UserEntity GetUserByInfo(string info)
        {
            if (info == null) return null;
            var user = _unitOfWork.UserRepository.Get(p => p.Email == info || p.Phone == info);
            if (user == null) return null;
            var model = Mapper.Map<User, UserEntity>(user);
            return model;
        }


        public UserEntity GetUserById(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            if (user == null)
                return null;
            var userEntity = Mapper.Map<User, UserEntity>(user);
            return userEntity;
        }


        public UserEntity UpdateAvatarThumbnail(UserEntity userEntity)
        {
            var user = _unitOfWork.UserRepository.GetById(userEntity.id);
            if (user == null)
                return null;
            using (var scope = new TransactionScope())
            {
                user.AvatarThumbnail = userEntity.avatarThumbnail;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
                scope.Complete();
                userEntity.id = user.Id;
                return userEntity;
            }
        }


        public UserEntity UpdateCoverThumbnail(UserEntity userEntity)
        {
            var user = _unitOfWork.UserRepository.GetById(userEntity.id);
            if (user == null)
                return null;
            using (var scope = new TransactionScope())
            {
                user.CoverThumbnail = userEntity.coverThumbnail;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
                scope.Complete();
                userEntity.id = user.Id;
                return userEntity;
            }
        }


        public UserEntity UpdateUser(int id, UserEntity userEntity)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            if (user == null)
                return null;
            using (var scope = new TransactionScope())
            {
                user = Mapper.Map<UserEntity, User>(userEntity);
                _unitOfWork.UserRepository.SetValue(id, user);
                _unitOfWork.Save();
                scope.Complete();
                userEntity.id = user.Id;
                return userEntity;
            }
        }
    }
}
