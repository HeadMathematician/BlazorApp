using Business.Repository.IRepository;
using HiddenVilla_Server.Areas.Identity.Pages;
using Microsoft.AspNetCore.Mvc;
using HiddenVilla_Server.Model;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Common;
using System.Globalization;

namespace HiddenVilla_Api.Controllers
{
    [Route("api/[controller]")]
    public class HotelRoomController : Controller
    {
        private readonly IHotelRoomRepository _hotelRoomRepository;

        public HotelRoomController(IHotelRoomRepository hotelRoomRepository)
        {
            _hotelRoomRepository = hotelRoomRepository;
        }
  
        [HttpGet]
        public async Task<IActionResult> GetHotelRooms(string checkInDate = null, string checkOutDate = null)
        {
            if(string.IsNullOrEmpty(checkInDate) || string.IsNullOrEmpty(checkOutDate))
            {
                return BadRequest(new ErrorModel()
                { 
                    ErrorMessage = "All parameters need to be supplied"
                });
            }
            if(!DateTime.TryParseExact(checkInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckInDate))
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid Check-In date format. Valid format is MM/dd/yyyy"
                });
            }
            if (!DateTime.TryParseExact(checkOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid Check-Out date format. Valid format is MM/dd/yyyy"
                });
            }
            var allRooms = await _hotelRoomRepository.GetAllHotelRoom(checkInDate, checkOutDate);
            return Ok(allRooms);
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetHotelRoom(int? roomId, string checkInDate = null, string checkOutDate = null)
        {
            if(roomId == null)
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "",
                    ErrorMessage = "Invalid Room Id",
                    Code = StatusCodes.Status400BadRequest
                });
            }

            if (string.IsNullOrEmpty(checkInDate) || string.IsNullOrEmpty(checkOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "All parameters need to be supplied"
                });
            }
            if (!DateTime.TryParseExact(checkInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckInDate))
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid Check-In date format. Valid format is MM/dd/yyyy"
                });
            }
            if (!DateTime.TryParseExact(checkOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid Check-Out date format. Valid format is MM/dd/yyyy"
                });
            }

            var roomDetails = await _hotelRoomRepository.GetHotelRoom(roomId.Value, checkInDate, checkOutDate);
            if(roomDetails == null)
            {
                return BadRequest(new ErrorModel
                {
                    Title = "",
                    ErrorMessage = "Invalid Room Id",
                    Code = StatusCodes.Status404NotFound
                });
            }

            return Ok(roomDetails);
        }

    }
}
