using HiddenVilla_Client.Service.IService;
using Models;
using System.Net.Http;

namespace HiddenVilla_Client.Service
{
    public class RoomOrderDetailsService : IRoomOrderDetailsService
    {
        private readonly HttpClient _client;
        public RoomOrderDetailsService(HttpClient client)
        {
            _client = client;
        }
        public Task<RoomOrderDetailsDTO> MarkPaymentSuccessful(RoomOrderDetailsDTO details)
        {
            throw new NotImplementedException();
        }

        public Task<RoomOrderDetailsDTO> SaveRoomOrderDetails(RoomOrderDetailsDTO details)
        {
            throw new NotImplementedException();
        }
    }
}
