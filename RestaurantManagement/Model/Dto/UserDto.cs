using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Api.Model.Dto
{
    public class UserDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
