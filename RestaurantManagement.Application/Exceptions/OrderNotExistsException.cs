namespace RestaurantManagement.Application.Exceptions
{
    public class OrderNotExistsException : Exception
    {
        public OrderNotExistsException(string message) : base(message)
        {
        }
    }
}
