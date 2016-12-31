using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace BusinessServices
{
    public interface IUserServices
    {
        UserEntity CreateUser(UserEntity userEntity);
        UserEntity GetUserByInfo(string info);
        UserEntity GetUserById(int id);
        UserEntity UpdateAvatarThumbnail(UserEntity userEntity);
        UserEntity UpdateCoverThumbnail(UserEntity userEntity);
        UserEntity UpdateUser(int id, UserEntity userEntity);
    }
}
