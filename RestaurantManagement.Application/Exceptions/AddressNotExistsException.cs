namespace RestaurantManagement.Application.Exceptions
{
    public class AddressNotExistsException : Exception
    {
        public AddressNotExistsException(string message) : base(message)
        {
        }
    }
}
