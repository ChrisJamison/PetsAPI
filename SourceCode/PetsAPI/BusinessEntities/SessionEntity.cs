using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class SessionEntity
    {
        public int id { get; set; }
        public string authToken { get; set; }
        public int userAuthInfoId { get; set; }
        public DateTime expiredOn { get; set; }
        public string deviceToken { get; set; }
        public bool isVerified { get; set; }
        public string deviceType { get; set; }
    }
}
