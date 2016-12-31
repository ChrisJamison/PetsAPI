using System;

namespace BusinessEntities
{
    public class UserEntity
    {
        public int id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string nationality { get; set; }
        public string languages { get; set; }
        public int? provinceId { get; set; }
        public int? districtId { get; set; }
        public int? wardId { get; set; }
        public string avatarThumbnail { get; set; }
        public string coverThumbnail { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? birthDate { get; set; }
        public DateTime? LastConnectedOn { get; set; }

    }
}
