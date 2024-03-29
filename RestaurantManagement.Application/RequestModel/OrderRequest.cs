namespace RestaurantManagement.Application.RequestModel
{
    public class OrderRequest
    {
        public List<string> PizzaNames { get; set; }
        public string UserId { get;set; }    
        public string AdressId { get;set; }
    }
}
