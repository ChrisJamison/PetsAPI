using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class OTPEntity
    {
        public int id { get; set; }
        public string otpCode { get; set; }
        public int userAuthInfoId { get; set; }
        public System.DateTime expiredOn { get; set; }
        public string type { get; set; }
    }
}
