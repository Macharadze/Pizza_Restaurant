using Mapster;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Infrastructure.Localizations;
using RestaurantManagement.Api.Model.Dto;
using RestaurantManagement.Application.Interfaces.IPizza;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Application.RequestModel;
using RestaurantManagement.Domain.Entity;
using RestaurantManagement.Infrastructure.Validators;

namespace RestaurantManagement.Controllers
{
    /// <summary>
    /// Controller for managing pizzas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PizzaController"/> class.
        /// </summary>
        /// <param name="pizzaRepository">The pizza repository.</param>
        /// <param name="userRepository"></param>
        public PizzaController(IPizzaRepository pizzaRepository, IUserRepository userRepository)
        {
            _pizzaRepository = pizzaRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets all pizzas.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of pizzas.</returns>
        /// <response code="200">Returns all the pizzas</response>
        [HttpGet]
        public async Task<ActionResult<List<PizzaDTO>>> GetAllPizzas(CancellationToken cancellationToken)
        {
          
            
                var pizzas = await _pizzaRepository.GetAll(cancellationToken);
                return Ok(pizzas.Adapt<List<PizzaDTO>>());
 
        }

        /// <summary>
        /// Gets a specific pizza by ID.
        /// </summary>
        /// <param name="id">The pizza ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The specified pizza.</returns>
        /// <response code="200">Returns the pizza by id</response>
        /// <response code="404">If the pizza is not valid</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<PizzaDTO>> GetPizzaById(string id, CancellationToken cancellationToken)
        {
            try
            {
                var pizza = await _pizzaRepository.GetById(cancellationToken, id);
                return Ok(pizza.Adapt<PizzaDTO>());
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Creates a new pizza.
        /// </summary>
        /// <param name="pizza">The pizza details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result indicating success or failure.</returns>
        ///  <response code="200">Returns newly created pizza</response>
        [HttpPost]
        public async Task<ActionResult> CreatePizza([FromBody] PizzaDTO pizza, CancellationToken cancellationToken)
        {
            try
            {
                var validation = new PizzaValidation();
                var result = validation.Validate(pizza);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }
                await _pizzaRepository.Create(cancellationToken, pizza.Adapt<Pizza>());
                return Ok(Language.Create);
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Updates a specific pizza by ID.
        /// </summary>
        /// <param name="id">The pizza ID to update.</param>
        /// <param name="pizza">The updated pizza details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result indicating success or failure.</returns>
        ///  <response code="200">Returns newly updated Pizza</response>
        /// <response code="404">If the pizza is not valid</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePizza(string id, [FromBody] PizzaRequest pizza, CancellationToken cancellationToken)
        {
            try
            {
                if (await _pizzaRepository.Update(cancellationToken, pizza.Adapt<Pizza>(), id))
                    return Ok(Language.Update);
                else
                    return NotFound(Language.NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Deletes a specific pizza by ID.
        /// </summary>
        /// <param name="id">The pizza ID to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result indicating success or failure.</returns>
        ///  <response code="200">Returns nwely deleted pizza</response>
        /// <response code="404">If the pizza is not valid</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePizza(string id, CancellationToken cancellationToken)
        {
            try
            {
                if (await _pizzaRepository.Delete(cancellationToken, id))
                    return Ok("Pizza deleted successfully");
                else
                    return NotFound(Language.NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Adds a rating to a specific pizza.
        /// </summary>
        /// <param name="rate">The rating details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result indicating success or failure.</returns>
        ///  <response code="200">added the pizza's rating by id</response>
        /// <response code="404">If the pizzaId or userId ais not valid</response>
        [HttpPost("{PizzaId}/rate")]
        public async Task<ActionResult> AddRating([FromBody] RankRequest rate, CancellationToken cancellationToken)
        {
            try
            {
                var validation = new RankValidation(_userRepository,_pizzaRepository);
                var result = await validation.ValidateAsync(rate);
                if(!result.IsValid)
                    return BadRequest(result.Errors);
            
                await _pizzaRepository.AddRate(cancellationToken, rate, rate.PizzaId, rate.UserId);
                return Ok(Language.Create);
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Gets the rating of a specific pizza.
        /// </summary>
        /// <param name="id">The pizza ID to retrieve the rating.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The rating of the specified pizza.</returns>
        ///  <response code="200">Returns the pizza  rating by id</response>
        /// <response code="404">If the pizza is not valid</response>
        [HttpGet("{id}/rating")]
        public async Task<ActionResult<double>> GetPizzaRating(string id, CancellationToken cancellationToken)
        {
            try
            {
                var rating = await _pizzaRepository.GetRating(cancellationToken, id);
                return Ok(rating);
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }
    }
}
