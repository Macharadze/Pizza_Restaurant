namespace RestaurantManagement.Application.Exceptions
{
    public class ImageDoesNotExistsException : Exception
    {
        public ImageDoesNotExistsException(string? message) : base(message)
        {
        }
    }
}
