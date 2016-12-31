using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;

namespace BusinessServices
{
    public class ArticleServices : IArticleServices
    {
        private readonly UnitOfWork _unitOfWork;

        public ArticleServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ArticleEntity CreateArticle(BusinessEntities.ArticleEntity articleEntity)
        {
            var article = Mapper.Map<ArticleEntity, Article>(articleEntity);
            using (var scope = new TransactionScope())
            {
                _unitOfWork.ArticleRepository.Insert(article);
                _unitOfWork.Save();
                scope.Complete();
                articleEntity.id = article.Id;
                return articleEntity;
            }
        }


        public IEnumerable<ArticleEntity> GetArticles()
        {
            var articles = _unitOfWork.ArticleRepository.GetAll().ToList();
            var articleEtities = Mapper.Map<List<Article>, List<ArticleEntity>>(articles);
            return articleEtities;
        }


        public ArticleEntity GetArticleById(int articleEntityId)
        {
            var article = _unitOfWork.ArticleRepository.GetById(articleEntityId);
            if (article == null)
                return null;
            var articleEntity = Mapper.Map<Article, ArticleEntity>(article);
            return articleEntity;
        }


        public ArticleEntity GetArticleByPetId(int petEntityId)
        {
            var article = _unitOfWork.ArticleRepository.Get(c => c.PetId == petEntityId);
            if (article == null)
                return null;
            var articleEntity = Mapper.Map<Article, ArticleEntity>(article);
            return articleEntity;
        }


        public IEnumerable<ArticleEntity> GetArticlesByInfo(string searchString)
        {
            var articles =
                _unitOfWork.ArticleRepository.GetMany(
                    c =>
                        CharacterHelper.MapUnicodeToAscii(c.Title.ToLower()).Contains(CharacterHelper.MapUnicodeToAscii(searchString.ToLower()))
                         ||
                        CharacterHelper.MapUnicodeToAscii(c.Description.ToLower()).Contains(CharacterHelper.MapUnicodeToAscii(searchString.ToLower()))
                        ).ToList();
            if (!articles.Any()) return null;
            var articleEntities = Mapper.Map<List<Article>, List<ArticleEntity>>(articles);
            return articleEntities;
        }


        public ArticleEntity UpdateArticle(int id, ArticleEntity articleEntity)
        {
            var article = _unitOfWork.ArticleRepository.GetById(id);
            if(article == null)
                return null;
            using (var scope = new TransactionScope())
            {
                article = Mapper.Map<ArticleEntity, Article>(articleEntity);
                _unitOfWork.ArticleRepository.SetArticleValue(id, article);
                _unitOfWork.Save();
                scope.Complete();
                return articleEntity;
            }
        }
    }
}
