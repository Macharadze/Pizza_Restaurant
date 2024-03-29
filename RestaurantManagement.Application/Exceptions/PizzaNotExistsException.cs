namespace RestaurantManagement.Application.Exceptions
{
    public class PizzaNotExistsException : Exception
    {
        public PizzaNotExistsException(string message) : base(message)
        {
        }
    }
}
