using Mapster;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Infrastructure.Localizations;
using RestaurantManagement.Api.Model.Dto;
using RestaurantManagement.Application.Interfaces.IOrder;
using RestaurantManagement.Application.Interfaces.IPizza;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Application.RequestModel;
using RestaurantManagement.Application.Response;
using RestaurantManagement.Domain.Entity;
using RestaurantManagement.Infrastructure.Validators;

namespace RestaurantManagement.Api.Controllers
{
    /// <summary>
    /// Controller for managing orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPizzaRepository _pizzaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderRepository">The order repository.</param>
        /// <param name="userRepository"></param>
        /// <param name="pizzaRepository"></param>
        public OrderController(IOrderRepository orderRepository,IUserRepository userRepository,IPizzaRepository pizzaRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _pizzaRepository = pizzaRepository; 
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of orders.</returns>
        ///  <response code="200">Returns all the orders</response>
        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetAllOrders(CancellationToken cancellationToken)
        { 
          
            var orders = await _orderRepository.GetAll(cancellationToken);
             List<OrderResponse> result = new List<OrderResponse>();
            foreach (var item in orders)
            {
                result.Add(new OrderResponse { PizzaNames = await _orderRepository.GetPizzas(cancellationToken, item.Id.ToString()) });
            }
            return Ok(result);
           
        }

        /// <summary>
        /// Gets a specific order by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The specified order.</returns>
        ///  <response code="200">Returns the order by id</response>
        /// <response code="404">If the order is not valid</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(string id, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepository.GetById(cancellationToken, id);
                return Ok(new OrderResponse { PizzaNames = await _orderRepository.GetPizzas(cancellationToken,id)});
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderRequest">The order request containing pizza details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result indicating success or failure.</returns>
        ///  <response code="200">create new order</response>
        [HttpPost("newOrder")]
        public async Task<ActionResult> CreateOrder([FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            try
            {
                var validation = new OrderValidation(_userRepository, _pizzaRepository);
                var result = await validation.ValidateAsync(orderRequest);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }
                var order = orderRequest.Adapt<Order>();
                await _orderRepository.CreateOrderPizaa(cancellationToken, orderRequest.PizzaNames, orderRequest.UserId, order.Id.ToString(), orderRequest.AdressId);
                await _orderRepository.Create(cancellationToken, order);
                return Ok(Language.Create);
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Deletes a specific order by ID.
        /// </summary>
        /// <param name="id">The order ID to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result indicating success or failure.</returns>
        ///  <response code="200">delete order by id</response>
        /// <response code="404">If the order is not valid</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(string id, CancellationToken cancellationToken)
        {
            try
            {
                if (await _orderRepository.Delete(cancellationToken, id))
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
        /// Gets the pizzas associated with a specific order.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="orderId">The ID of the order to retrieve pizzas.</param>
        /// <returns>The list of pizzas associated with the order.</returns>
        ///  <response code="200">Returns the pizza by id</response>
        /// <response code="404">If the pizza is not valid</response>
        ///  <response code="200">return pizzas by orderid</response>
        /// <response code="404">If the order is not valid</response>
        [HttpGet("order")]
        public async Task<ActionResult> GetPizzas(CancellationToken cancellationToken, string orderId)
        {
            try
            {
                return Ok(await _orderRepository.GetPizzas(cancellationToken, orderId));
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }
    }
}
