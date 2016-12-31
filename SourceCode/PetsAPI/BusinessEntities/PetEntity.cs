using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class PetEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime? birthDate { get; set; }
        public decimal? price { get; set; }
        public int imageId { get; set; }
        public int userAuthInfoId { get; set; }
    }
}
