using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Business.Repository
{
    public class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public HotelRoomRepository(ApplicationDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }
        public async Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto hotelRoomDTO)
        {
            HotelRoom hotelRoom = _mapper.Map<HotelRoomDto, HotelRoom>(hotelRoomDTO);
            hotelRoom.CreatedDate = DateTime.UtcNow;
            hotelRoom.CreatedBy = "";
            var addedHotelRoom = await _db.HotelRooms.AddAsync(hotelRoom);
            await _db.SaveChangesAsync();
            return _mapper.Map<HotelRoom, HotelRoomDto>(addedHotelRoom.Entity);
        }

        public async Task<int> DeleteHotelRoom(int roomId)
        {
            var roomDetails = await _db.HotelRooms.FindAsync(roomId);
            if(roomDetails != null)
            {
                var allImages = await _db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync();
                
                _db.HotelRoomImages.RemoveRange(allImages);
                _db.HotelRooms.Remove(roomDetails);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<HotelRoomDto>> GetAllHotelRoom(string checkInDateStr, string checkOutDateStr)
        {
            try
            {
                IEnumerable<HotelRoomDto> hotelRoomDtos = await _db.HotelRooms
                    .Include(x => x.HotelRoomImages)
                    .Select(room => _mapper.Map<HotelRoom, HotelRoomDto>(room))
                    .ToListAsync();

                return hotelRoomDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<HotelRoomDto> GetHotelRoom(int roomId, string checkInDateStr, string checkOutDateStr)
        {
            try
            {
                HotelRoomDto hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                    await _db.HotelRooms.Include(x=>x.HotelRoomImages).FirstOrDefaultAsync(x => x.Id == roomId));
                return hotelRoom;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<HotelRoomDto> isRoomNameTaken(string name, int roomId = 0)
        {
            try
            {
                if (roomId == 0)
                {


                    HotelRoomDto hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                        await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()));
                    return hotelRoom;
                }
                else
                {
                    HotelRoomDto hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                        await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.Id!=roomId));
                    return hotelRoom;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HotelRoomDto> UpdateHotelRoom(int roomId, HotelRoomDto hotelRoomDto)
        {
            try
            {
                if (roomId == hotelRoomDto.Id)
                {
                    HotelRoom roomDetails = await _db.HotelRooms.FindAsync(roomId);
                    HotelRoom room = _mapper.Map<HotelRoomDto, HotelRoom>(hotelRoomDto, roomDetails);
                    room.UpdatedBy = "";
                    room.UpdatedDate = DateTime.Now;
                    var updatedRoom = _db.HotelRooms.Update(room);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<HotelRoom, HotelRoomDto>(updatedRoom.Entity);
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }
    }
}
