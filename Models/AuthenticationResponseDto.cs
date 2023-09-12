using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AuthenticationResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string Errors { get; set; }
        public string Token { get; set; }
        public UserDto userDto { get; set; }
    }
}
