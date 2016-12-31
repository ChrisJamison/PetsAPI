using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsAPI.Models
{
    public class PetExpandedModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime? birthDate { get; set; }
        public decimal? price { get; set; }
        public int imageId { get; set; }
        public int userAuthInfoId { get; set; }
        public string thumbNailInBase64 { get; set; }
    }
}