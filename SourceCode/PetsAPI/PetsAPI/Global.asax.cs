using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using BusinessEntities;
using DataModel;
using PetsAPI.Models;

namespace PetsAPI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CreateMap();
        }

        protected void CreateMap()
        {
            Mapper.CreateMap<UserEntity, User>();
            Mapper.CreateMap<UserModel, UserEntity>();
            Mapper.CreateMap<UserAuthInfoEntity, UserAuthInfo>();
            Mapper.CreateMap<SessionEntity, Session>();
            Mapper.CreateMap<User, UserEntity>();
            Mapper.CreateMap<UserAuthInfo, UserAuthInfoEntity>();
            Mapper.CreateMap<OTPEntity, OTP>();
            Mapper.CreateMap<ArticleEntity, Article>();
            Mapper.CreateMap<Pet, PetEntity>();
            Mapper.CreateMap<PetExpandedModel, PetEntity>();
            Mapper.CreateMap<PetEntity, Pet>();
            Mapper.CreateMap<ImageEntity, Image>();
            Mapper.CreateMap<OTP, OTPEntity>();
            Mapper.CreateMap<Image, ImageEntity>();
            Mapper.CreateMap<Article, ArticleEntity>();
            Mapper.CreateMap<ArticleEntity, ArticleExpandedModel>();
            Mapper.CreateMap<PetEntity, PetModel>();
        }
    }
}
