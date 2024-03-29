using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Api.Model.Dto
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public List<Pizza> Pizzas { get; set; }
    }
}
