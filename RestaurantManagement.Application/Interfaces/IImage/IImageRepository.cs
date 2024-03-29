using RestaurantManagement.Application.Interfaces.IBase;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Application.Interfaces.IImage
{
    public interface IImageRepository : IRepository<Image>
    {
        string GetFilePath(string filePath);
    }
}
