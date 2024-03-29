using Mapster;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Infrastructure.Localizations;
using RestaurantManagement.Api.Model.Dto;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Domain.Entity;
using RestaurantManagement.Infrastructure.Validators;

namespace RestaurantManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/>
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <response code="200">Returns All the users</response>
        [HttpGet("Users")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetAll(cancellationToken);
                var result = users.Select(i => i.Adapt<UserDto>());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Get a user by ID.
        /// </summary>
        /// <response code="200">Returns the user by id</response>
        /// <response code="404">If the user is not valid</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetById(cancellationToken, id);
                return Ok(user.Adapt<UserDto>());
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Update a user by ID.
        /// </summary>
        /// <response code="200">Returns the newly updated user</response>
        /// <response code="404">If the user is not valid</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] UserDto user, CancellationToken cancellationToken)
        {
            try
            {
                if (await _userRepository.Update(cancellationToken, user.Adapt<User>(), id))
                    return Ok("User updated successfully");
                else
                    return NotFound(Language.NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(404, "Internal Server Error");
            }
        }

        /// <summary>
        /// Delete a user by ID.
        /// </summary>
        /// <response code="200">Returns the newly updatdeleteded user</response>
        /// <response code="404">If the user is not valid</response>
        [HttpDelete("deletedItem/{id}")]
        public async Task<ActionResult> DeleteUser(string id, CancellationToken cancellationToken)
        {
            try
            {
                if (await _userRepository.Delete(cancellationToken, id))
                    return Ok(Language.Delete);
                else
                    return NotFound(Language.NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Get addresses associated with a user by ID.
        /// </summary>
        /// <response code="200">Returns the user's addresses</response>
        /// <response code="404">If the user is not valid</response>
        [HttpGet("{id}/addresses")]
        public async Task<ActionResult<List<AddressDto>>> GetUserAddresses(string id, CancellationToken cancellationToken)
        {
            try
            {
                var addresses = await _userRepository.GetAddresses(cancellationToken, id);
                return Ok(addresses.Adapt<List<AddressDto>>());
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Add a new user.
        /// </summary>
        /// <response code="200">Returns the newly added user</response>
        [HttpPost("AddUser")]
        public async Task<ActionResult> AddUser(UserDto user)
        {
            var validation = new UserValidation();
            var res = validation.Validate(user);
            if (!res.IsValid)
                return BadRequest(res.Errors);

            await _userRepository.Create(new CancellationToken(), user.Adapt<User>());
            return Ok(Language.Create);
        }
    }
}
