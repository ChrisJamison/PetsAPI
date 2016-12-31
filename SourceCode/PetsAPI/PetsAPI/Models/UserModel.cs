using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsAPI.Models
{
    public class UserModel
    {
        public string email { get; set; }
        public string phone { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string passwordHash { get; set; }
        public string info { get; set; }
        public string oTPCode { get; set; }
        public string avatarThumbnailInBase64 { get; set; }
        public string coverThumbnailInBase64 { get; set; }
    }
}