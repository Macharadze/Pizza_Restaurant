namespace RestaurantManagement.Application.RequestModel
{
    public class AddressRequest
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }
}
