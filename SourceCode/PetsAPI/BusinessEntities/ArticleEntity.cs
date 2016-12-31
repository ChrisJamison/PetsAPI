using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class ArticleEntity
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string content { get; set; }
        public decimal? price { get; set; }
        public int petId { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? modifiedOn { get; set; }
        public int? view { get; set; }
    }
}
