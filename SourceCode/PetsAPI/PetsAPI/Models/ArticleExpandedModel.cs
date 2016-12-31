using BusinessEntities;

namespace PetsAPI.Models
{
    public class ArticleExpandedModel: ArticleEntity
    {
        public PetModel Pet { get; set; }
    }

    public class PetModel : PetEntity
    {
        public ImageEntity Image { get; set; }

        public UserEntity User { get; set; }
    }
}