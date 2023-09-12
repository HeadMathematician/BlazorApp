using Models;

namespace HiddenVilla_Client.Service.IService
{
    public interface IHotelRoomService
    {
        public Task<IEnumerable<HotelRoomDto>> GetHotelRooms(string checkInDate, string checkOutDate);
        public Task<HotelRoomDto> GetHotelRoomDetails(int roomId, string checkInDate, string checkOutDate);
    }
}
