using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace BusinessServices
{
    public interface IArticleServices
    {
        ArticleEntity CreateArticle(ArticleEntity articleEntity);
        IEnumerable<ArticleEntity> GetArticles();
        ArticleEntity GetArticleById(int articleEntityId);
        ArticleEntity GetArticleByPetId(int petEntityId);
        IEnumerable<ArticleEntity> GetArticlesByInfo(string searchString);
        ArticleEntity UpdateArticle(int id, ArticleEntity articleEntity);
    }
}
