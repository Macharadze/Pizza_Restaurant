using Mapster;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Infrastructure.Localizations;
using RestaurantManagement.Api.Model.Dto;
using RestaurantManagement.Application.Interfaces.IAddress;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Application.RequestModel;
using RestaurantManagement.Domain.Entity;
using RestaurantManagement.Infrastructure.Validators;

namespace RestaurantManagement.Api.Controllers
{
    /// <summary>
    /// Controller for managing addresses.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressController"/> class.
        /// </summary>
        /// <param name="addressRepository">The address repository.</param>
        /// <param name="userRepository"></param>
        public AddressController(IAddressRepository addressRepository, IUserRepository userRepository)
        {
            _addressRepository = addressRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets all addresses.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of address DTOs.</returns>
        ///  <response code="200">returns all the addresses</response>
        [HttpGet]
        public async Task<ActionResult<List<AddressDto>>> GetAllAddresses(CancellationToken cancellationToken)
        {
              var addresses = await _addressRepository.GetAll(cancellationToken);
              var addressDtos = addresses.Select(a => a.Adapt<AddressDto>());
              return Ok(addressDtos);
        }

        /// <summary>
        /// Gets an address by its ID.
        /// </summary>
        /// <param name="id">The ID of the address.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Address DTO.</returns>
        /// <response code="200">returns the address by id</response>
        /// <response code="404">if address not exists</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDto>> GetAddressById(string id, CancellationToken cancellationToken)
        {
            try
            {
                var address = await _addressRepository.GetById(cancellationToken, id);
                return Ok(address.Adapt<AddressDto>());
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Creates a new address.
        /// </summary>
        /// <param name="id">The ID of the user associated with the address.</param>
        /// <param name="addressRequest">The address DTO to be created.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result of the address creation operation.</returns>
        /// <response code="200">create new address</response>
        [HttpPost]
        public async Task<ActionResult> CreateAddress([FromBody] AddressRequest addressRequest, CancellationToken cancellationToken)
        {
            try
            {
                var validation = new AddressValidation(_userRepository);
                var result = await validation.ValidateAsync(addressRequest);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                var address = addressRequest.Adapt<Address>();
                address.UserId = Guid.Parse(addressRequest.UserId);
                await _addressRepository.Create(cancellationToken, address);
                return Ok(Language.Create);
            }
            catch (Exception ex)
            {
                return StatusCode(404 ,Language.NotFound);
            }
        }

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="id">The ID of the address to be updated.</param>
        /// <param name="address">The updated address data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result of the address update operation.</returns>
        /// <response code="200">update the address</response>
        /// <response code="404">if address not exists</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAddress(string id, [FromBody] AddressDto address, CancellationToken cancellationToken)
        {
            try
            {
                if (await _addressRepository.Update(cancellationToken, address.Adapt<Address>(), id))
                    return Ok(Language.Update);
                else
                    return NotFound(Language.NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.Conflict);
            }
        }

        /// <summary>
        /// Deletes an address by its ID.
        /// </summary>
        /// <param name="id">The ID of the address to be deleted.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result of the address deletion operation.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAddress(string id, CancellationToken cancellationToken)
        {
            try
            {
                if (await _addressRepository.Delete(cancellationToken, id))
                    return Ok(Language.Delete);
                else
                    return NotFound(Language.NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.Conflict);
            }
        }
    }
}
