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
    public class ImageServices : IImageServices
    {
        private readonly UnitOfWork _unitOfWork;

        public ImageServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BusinessEntities.ImageEntity CreateImage(BusinessEntities.ImageEntity imageEntity)
        {
            var image = Mapper.Map<ImageEntity, Image>(imageEntity);
            using (var scope = new TransactionScope())
            {
                _unitOfWork.ImageRepository.Insert(image);
                _unitOfWork.Save();
                scope.Complete();
                imageEntity.id = image.Id;
                return imageEntity;
            }
        }


        public ImageEntity GetImageById(int imageEntityId)
        {
            var image = _unitOfWork.ImageRepository.GetById(imageEntityId);
            if (image == null)
                return null;
            var imageEntity = Mapper.Map<Image, ImageEntity>(image);
            return imageEntity;
        }


        public IEnumerable<ImageEntity> GetAll()
        {
            var images = _unitOfWork.ImageRepository.GetAll().ToList();
            if (!images.Any())
                return null;
            var imageEntities = Mapper.Map<List<Image>, List<ImageEntity>>(images);
            return imageEntities;
        }
    }
}
