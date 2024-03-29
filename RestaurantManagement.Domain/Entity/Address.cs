namespace RestaurantManagement.Domain.Entity
{
    public class Address : BaseClass
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }

    }
}
