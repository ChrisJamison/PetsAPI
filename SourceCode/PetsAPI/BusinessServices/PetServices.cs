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
    public class PetServices : IPetServices
    {
        private readonly UnitOfWork _unitOfWork;

        public PetServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BusinessEntities.PetEntity GetPetById(int id)
        {
            var pet = _unitOfWork.PetRepository.GetById(id);
            if (pet == null)
                return null;
            var petEntity = Mapper.Map<Pet, PetEntity>(pet);
            return petEntity;
        }


        public PetEntity CreatePet(PetEntity petEntity)
        {
            var pet = Mapper.Map<PetEntity, Pet>(petEntity);
            using (var scope = new TransactionScope())
            {
                _unitOfWork.PetRepository.Insert(pet);
                _unitOfWork.Save();
                scope.Complete();
                petEntity.id = pet.Id;
                return petEntity;
            }
        }


        public IEnumerable<PetEntity> GetAll()
        {
            var pets = _unitOfWork.PetRepository.GetAll().ToList();
            if (!pets.Any())
                return null;
            var petEntities = Mapper.Map<List<Pet>, List<PetEntity>>(pets);
            return petEntities;
        }


        public IEnumerable<PetEntity> GetPetsByUserAuthInfoId(int userAuthInfoId)
        {
            var pets = _unitOfWork.PetRepository.GetMany(c => c.UserAuthInfoId == userAuthInfoId).ToList();
            if (!pets.Any())
                return null;
            var petEntities = Mapper.Map<List<Pet>, List<PetEntity>>(pets);
            return petEntities;
        }
    }
}
