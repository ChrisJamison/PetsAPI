using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BusinessEntities;
using BusinessServices;
using DataModel;
using PetsAPI.Constants;
using PetsAPI.Filters;
using PetsAPI.Helpers;
using PetsAPI.Models;

namespace PetsAPI.Controllers.Api
{
    public enum OrderType
    {
        Feature,
        New
    }
    [AuthorizationRequired]
    public class ArticlesController : ApiController
    {
        private readonly IArticleServices _iArticleServices;
        private readonly IPetServices _iPetServices;
        private readonly IImageServices _iImageServices;
        private readonly IUserServices _iUserServices;
        private readonly IUserAuthInfoServices _iUserAuthInfoServices;
        public ArticlesController(IArticleServices iArticleServices, IPetServices iPetServices, IImageServices iImageServices, IUserServices iUserServices, IUserAuthInfoServices iUserAuthInfoServices)
        {
            _iArticleServices = iArticleServices;
            _iPetServices = iPetServices;
            _iImageServices = iImageServices;
            _iUserServices = iUserServices;
            _iUserAuthInfoServices = iUserAuthInfoServices;
        }

        public HttpResponseMessage Post([FromBody] ArticleEntity articleEntity)
        {
            try
            {
                var pet = _iPetServices.GetPetById(articleEntity.petId);
                if (pet == null)
                {
                    return ResponseHelper.ErrorResult(Request, HttpStatusCode.BadRequest,
                        ErrorMessages.ERROR_MSG_PET_NOT_EXISTED, ErrorCodeStrings.PET_IS_NOT_EXISTED, null);
                }
                articleEntity.createdOn = DateTime.Now;
                articleEntity.view = 0;
                var article = _iArticleServices.CreateArticle(articleEntity);
                return Request.CreateResponse(HttpStatusCode.OK, article);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [Route("api/articles/{order}/{skip}/{take}")]
        [HttpGet]
        public HttpResponseMessage GetArticles(int order, int skip, int take)
        {
            // order 1: newest, 0: featurest
            try
            {
                List<ArticleExpandedModel> articles;
                if ((OrderType)order == OrderType.Feature)
                {
                    var articlesResult = _iArticleServices.GetArticles().OrderByDescending(c => c.view).Skip(skip).Take(take).ToList();
                    articles = Mapper.Map<List<ArticleEntity>, List<ArticleExpandedModel>>(articlesResult);
                    var pets = _iPetServices.GetAll();
                    var petEntities = pets as IList<PetEntity> ?? pets.ToList();
                    foreach (var article in articles)
                    {
                        foreach (var pet in petEntities.Where(pet => article.petId == pet.id))
                        {
                            article.Pet = Mapper.Map<PetEntity, PetModel>(pet);
                            var userAuthInfo = _iUserAuthInfoServices.GetUserAuthInforById(pet.userAuthInfoId);
                            article.Pet.User = _iUserServices.GetUserById(userAuthInfo.userId);
                            var images = _iImageServices.GetAll();
                            foreach (var image in images.Where(image => article.Pet.imageId == image.id))
                            {
                                article.Pet.Image = image;
                            }
                        }

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, articles);
                }

                var newArticlesResult = _iArticleServices.GetArticles().OrderByDescending(c => c.view).Skip(skip).Take(take).ToList();
                articles = Mapper.Map<List<ArticleEntity>, List<ArticleExpandedModel>>(newArticlesResult);
                var newPets = _iPetServices.GetAll();
                var newPetEntities = newPets as IList<PetEntity> ?? newPets.ToList();
                foreach (var article in articles)
                {
                    foreach (var pet in newPetEntities.Where(pet => article.petId == pet.id))
                    {
                        article.Pet = Mapper.Map<PetEntity, PetModel>(pet);
                        var userAuthInfo = _iUserAuthInfoServices.GetUserAuthInforById(pet.userAuthInfoId);
                        article.Pet.User = _iUserServices.GetUserById(userAuthInfo.userId);
                        var images = _iImageServices.GetAll();
                        foreach (var image in images.Where(image => article.Pet.imageId == image.id))
                        {
                            article.Pet.Image = image;
                        }
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, articles);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [Route("api/articles/{id}/detail")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var article = _iArticleServices.GetArticleById(id);
                var result = Mapper.Map<ArticleEntity, ArticleExpandedModel>(article);
                if (article == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                var pet = _iPetServices.GetPetById(result.petId);
                result.Pet = Mapper.Map<PetEntity, PetModel>(pet);
                if (pet == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                var userAuthInfo = _iUserAuthInfoServices.GetUserAuthInforById(pet.userAuthInfoId);
                result.Pet.User = _iUserServices.GetUserById(userAuthInfo.userId);
                var image = _iImageServices.GetImageById(pet.imageId);
                result.Pet.Image = image;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [Route("api/articles/search/{searchString}")]
        [HttpGet]
        public HttpResponseMessage Search(string searchString)
        {
            try
            {
                var articlesResult = _iArticleServices.GetArticlesByInfo(searchString);
                if (articlesResult == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                var articles = Mapper.Map<List<ArticleEntity>, List<ArticleExpandedModel>>(articlesResult.ToList());
                var pets = _iPetServices.GetAll();
                var petEntities = pets as IList<PetEntity> ?? pets.ToList();
                foreach (var article in articles)
                {
                    foreach (var pet in petEntities.Where(pet => article.petId == pet.id))
                    {
                        article.Pet = Mapper.Map<PetEntity, PetModel>(pet);
                        var userAuthInfo = _iUserAuthInfoServices.GetUserAuthInforById(pet.userAuthInfoId);
                        article.Pet.User = _iUserServices.GetUserById(userAuthInfo.userId);
                        var images = _iImageServices.GetAll();
                        foreach (var image in images.Where(image => article.Pet.imageId == image.id))
                        {
                            article.Pet.Image = image;
                        }
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, articles);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }

        [Route("api/articles/{id}/updateview")]
        [HttpPut]
        public HttpResponseMessage UpdateView(int id)
        {
            try
            {
                var articleEntity = _iArticleServices.GetArticleById(id);
                if (articleEntity == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                articleEntity.view++;
                var result = _iArticleServices.UpdateArticle(id, articleEntity);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return ResponseHelper.ErrorResult(Request, HttpStatusCode.InternalServerError, ErrorMessages.ERROR_MSG_SERVER_ERROR, ErrorCodeStrings.SERVER_ERROR, e.ToString());
            }
        }
    }
}
