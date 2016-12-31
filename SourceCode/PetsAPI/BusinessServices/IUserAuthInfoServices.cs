using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace BusinessServices
{
    public interface IUserAuthInfoServices
    {
        UserAuthInfoEntity CreateUserAuthInfo(UserAuthInfoEntity userAuthInfoEntity);
        UserAuthInfoEntity GetUserAuthInfoByUserId(int id);
        UserAuthInfoEntity UpdateUserAuthInfoEntity(int id, UserAuthInfoEntity userAuthInfoEntity);
        UserAuthInfoEntity GetUserAuthInforById(int id);
    }
}
