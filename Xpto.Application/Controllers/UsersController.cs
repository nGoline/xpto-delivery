using System.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Xpto.Domain.Services;
using Xpto.Domain.DTO;
using Xpto.Domain.Interfaces.Service;

namespace Xpto.Application.Controllers
{
    [Route("api/[controller]"),
     ApiController,
     Authorize]
    public class UsersController : Controller
    {
        private bool _wasCreated = false;
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // [HttpGet]
        // public IActionResult Login(string returnUrl = "/")
        // {
        //     return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        // }

        [HttpGet("authenticate"),
         AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Authenticate(string username, string password)
        {
            var user = await _userService.AuthenticateAsync(username, password);

            if (user == null)
                return NotFound();

            var userDTO = new UserDTO(user);

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("_appSettings.Secret");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userDTO.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return await Task.FromResult(userDTO);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> ListUsers()
        {
            var users = await _userService.GetAllAsync();
            var usersDTO = new List<UserDTO>(users.Count());

            if (users.Count() == 0)
                return NoContent();

            foreach (var user in users)
            {
                usersDTO.Add(new UserDTO(user));
            }

            return Ok(usersDTO);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> FindUser(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
                return NotFound("User not found.");

            return Ok(new UserDTO(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(UserDTO userDTO)
        {
            var user = userDTO.ToEntity();
            await _userService.AddAsync(user);

            return Ok(new UserDTO(user));
        }

        [HttpPut]
        public async Task<ActionResult<UserDTO>> UpdateUser(UserDTO userDTO)
        {
            if (userDTO.Id == null || userDTO.Id == Guid.Empty)
                return StatusCode(500, $"{nameof(userDTO.Id)} can't be empty.");
            
            var user = userDTO.ToEntity();

            await _userService.UpdateAsync(user);

            return Ok(new UserDTO(user));
        }
    }
}