namespace RestaurantManagement.Application.Exceptions
{
    public class UserNotExistsException : Exception
    {
        public UserNotExistsException(string message) : base(message)
        {
        }
    }
}
