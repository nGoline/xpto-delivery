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
     Authorize,
     Produces("application/json")]
    public class UsersController : Controller
    {
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
        ///     GET /api/Users/Authenticate
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
         AllowAnonymous,
         ProducesResponseType(201),
         ProducesResponseType(400),
         ProducesResponseType(404)]
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
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Users/
        ///
        /// </remarks>
        /// <returns>List of Users</returns>
        /// <response code="200">Returns the User List</response>
        /// <response code="401">User unauthorized</response>
        /// <response code="204">The database is empty</response>
        [HttpGet,
         ProducesResponseType(200),
         ProducesResponseType(401),
         ProducesResponseType(204)]
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
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Users/00000000-0000-0000-0000-000000000000
        ///
        /// </remarks>
        /// <returns>User Object</returns>
        /// <response code="200">Returns the User</response>
        /// <response code="401">User unauthorized</response>
        /// <response code="404">User not found</response>
        [HttpGet("{userId}"),
         ProducesResponseType(204),
         ProducesResponseType(401),
         ProducesResponseType(404)]
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
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Users/
        ///     {
        ///       "name": "Mike",
        ///       "email": "mike@xpto.com",
        ///       "password": "123456"
        ///     }
        ///
        /// </remarks>
        /// <returns>Created User Object</returns>
        /// <response code="200">Returns the created User</response>
        /// <response code="401">User unauthorized</response>
        /// <response code="500">Error creating User</response>
        [HttpPost,
         ProducesResponseType(200),
         ProducesResponseType(401),
         ProducesResponseType(500)]
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
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Users/
        ///
        /// </remarks>
        /// <returns>Updated User Object</returns>
        /// <response code="200">Returns the updated User</response>
        /// <response code="401">User unauthorized</response>
        /// <response code="500">Error updating User</response>
        [HttpPut,
         ProducesResponseType(200),
         ProducesResponseType(401),
         ProducesResponseType(500)]
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