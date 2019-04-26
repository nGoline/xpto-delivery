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
using Xpto.Domain.Entities;
using Xpto.Domain.Validation;

namespace Xpto.Application.Controllers
{
    /// <summary>
    /// Users API relates to User actions such as
    /// <c>Create</c>, <c>Read</c>, <c>Update</c>, <c>Delete</c>
    /// and also <c>Authentication</c> features.
    /// </summary>
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

        /// <summary>
        /// Authenticates a User
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Users/Authenticate
        ///     {
        ///        "email": "mike@xpto.com",
        ///        "password": "123456"
        ///     }
        ///
        /// </remarks>
        /// <returns>User Object containing a token to be used on other calls</returns>
        /// <response code="200">Returns the Logged in User</response>
        /// <response code="400">Client sent a bad request</response>
        /// <response code="404">If the combination of <c>email</c> and <c>password</c> didn't return a valid User</response>       
        [HttpGet("authenticate"),
         AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Authenticate(string email, string password)
        {
            var user = new User("login", email, password);
            ValidationResult validationResult;
            (user, validationResult) = await _userService.AuthenticateAsync(user);

            if (!validationResult.IsValid)
                return StatusCode(user == null ? 404 : 400, validationResult.Errors);

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

            return await Task.FromResult(userDTO);
        }

        /// <summary>
        /// Fetch all Users
        /// </summary>
        /// <returns>A list of all Users</returns>
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

        /// <summary>
        /// Find a specific User by its Id
        /// </summary>
        /// <param name="userId">User Id to be found</param>
        /// <returns>User Object</returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> FindUser(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
                return NotFound("User not found.");

            return Ok(new UserDTO(user));
        }

        /// <summary>
        /// Creates a new User
        /// </summary>
        /// <param name="userDTO">User Object to be created</param>
        /// <returns>Created User Object</returns>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(UserDTO userDTO)
        {
            var user = userDTO.ToEntity();
            await _userService.AddAsync(user);

            return Ok(new UserDTO(user));
        }

        /// <summary>
        /// Updates an existing User
        /// </summary>
        /// <param name="userDTO">User Object to be updated</param>
        /// <returns>Updated User Object</returns>
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