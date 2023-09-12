using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IHotelImagesRepository
    {
        public Task <int> CreateHotelRoomImage(HotelRoomImageDto image);
        public Task <int> DeleteHotelRoomImageByImageId(int imageId);
        public Task <int> DeleteHotelRoomImageByRoomId(int roomId);
        public Task <int> DeleteHotelRoomImageByImageUrl(string imageUrl);
        public Task <IEnumerable<HotelRoomImageDto>> GetHotelRoomImages(int roomId);
    }
}
