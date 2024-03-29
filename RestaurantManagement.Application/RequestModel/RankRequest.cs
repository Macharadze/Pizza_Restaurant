namespace RestaurantManagement.Application.RequestModel
{
    public class RankRequest
    {
        public string PizzaId { get; set; }
        public string UserId { get; set; }

        public int Rate { get; set; }
    }
}
