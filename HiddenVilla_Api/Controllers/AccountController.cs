using Common;
using DataAccess.Data;
using HiddenVilla_Api.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HiddenVilla_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly APISettings _APISettings;
        public AccountController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
            ,IOptions<APISettings> options)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _APISettings = options.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserRequestDto userRequestDto)
        {
            if(userRequestDto == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new ApplicationUser
            {
                UserName = userRequestDto.Email,
                Email = userRequestDto.Email,
                Name = userRequestDto.Name,
                PhoneNumber = userRequestDto.PhoneNo,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRequestDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDto { Errors = errors, isRegistrationSuccessful = false });
            }
            var roleResult = await _userManager.AddToRoleAsync(user, SD.Role_Customer);
            if (!roleResult.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDto { Errors = errors, isRegistrationSuccessful = false });
            }
            return StatusCode(201);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] AuthenticationDto authenticationDto)
        {
            var result = await _signInManager.PasswordSignInAsync(authenticationDto.UserName,
                authenticationDto.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(authenticationDto.UserName);
                if(user == null)
                {
                    return Unauthorized(new AuthenticationResponseDto
                    {
                        IsAuthSuccessful = false,
                        Errors = "Invalid Authentication"
                    });
                }
                //everything is valid and we need to login the user

                var signingCredentials = GetSigningCredentials();
                var claims = await GetClaims(user);

                var tokenOptions = new JwtSecurityToken(
                    issuer: _APISettings.ValidIssuer,
                    audience: _APISettings.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: signingCredentials);

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new AuthenticationResponseDto
                {
                    IsAuthSuccessful = true,
                    Token = token,
                    userDto = new UserDto
                    {
                        Name = user.Name,
                        Id = user.Id,
                        Email = user.Email,
                        PhoneNo = user.PhoneNumber
                    }
                });
            }
            else
            {
                return Unauthorized(new AuthenticationResponseDto
                {
                    IsAuthSuccessful = false,
                    Errors = "Invalid Authentication"
                });
            }
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_APISettings.SecretKey));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id),
            };
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        
    }
}
