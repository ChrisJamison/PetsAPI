using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace BusinessServices
{
    public interface IPetServices
    {
        PetEntity GetPetById(int id);
        PetEntity CreatePet(PetEntity petEntity);
        IEnumerable<PetEntity> GetAll();
        IEnumerable<PetEntity> GetPetsByUserAuthInfoId(int userAuthInfoId);
    }
}
