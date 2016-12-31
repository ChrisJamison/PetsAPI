using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class UserAuthInfoEntity
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string passwordHash { get; set; }
    }
}
