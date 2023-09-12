using AutoMapper;
using DataAccess.Data;
using DataAcesss.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HotelRoomDto, HotelRoom>();
            CreateMap<HotelRoom, HotelRoomDto>();

            CreateMap<HotelRoomImage, HotelRoomImageDto>().ReverseMap();
            CreateMap<RoomOrderDetails, RoomOrderDetailsDTO>().ReverseMap();
        }
    }
}
