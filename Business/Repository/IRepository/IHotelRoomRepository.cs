using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IHotelRoomRepository
    {
        public Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto hotelRoomDTO);
        public Task<HotelRoomDto> UpdateHotelRoom(int roomId, HotelRoomDto hotelRoomDTO);
        public Task<HotelRoomDto> GetHotelRoom(int roomId, string checkInDate = null, string checkOutDate = null);
        public Task<int> DeleteHotelRoom(int roomId);

        public Task<IEnumerable<HotelRoomDto>> GetAllHotelRoom(string checkInDate = null, string checkOutDate = null);
        public Task<HotelRoomDto> isRoomNameTaken(string name, int roomId = 0);
    }
}
