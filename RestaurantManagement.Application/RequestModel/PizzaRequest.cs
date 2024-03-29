namespace RestaurantManagement.Application.RequestModel
{
    public class PizzaRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CaloryCount { get; set; }
    }
}
