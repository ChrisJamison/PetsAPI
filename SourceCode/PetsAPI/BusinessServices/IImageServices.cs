using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace BusinessServices
{
    public interface IImageServices
    {
        ImageEntity CreateImage(ImageEntity imageEntity);
        ImageEntity GetImageById(int imageEntityId);
        IEnumerable<ImageEntity> GetAll(); 
    }
}
