using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace BusinessServices
{
    public interface ISessionServices
    {
        SessionEntity CreateSession(SessionEntity sessionEntity);
        bool ValidateSession(string authToken);
    }
}
