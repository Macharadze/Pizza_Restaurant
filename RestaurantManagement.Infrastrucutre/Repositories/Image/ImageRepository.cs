using RestaurantManagement.Application.Exceptions;
using RestaurantManagement.Application.Interfaces.IImage;
using RestaurantManagement.Application.Interfaces.IPizza;
using RestaurantManagement.Domain.Database;
using RestaurantManagement.Domain.Entity;
using System.CodeDom.Compiler;

namespace RestaurantManagement.Infrastrucutre.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _database;
        private readonly IPizzaRepository _pizzaRepository;
        public ImageRepository(ApplicationDbContext db, IPizzaRepository pizzaRepository)
        {
            _database = db;
            _pizzaRepository = pizzaRepository;
        }


        public async Task<bool> Delete(CancellationToken cancellationToken, string path)
        {
            var image = _database.Images.Where(i => i.Path.Equals(path)).FirstOrDefault();
            if (image != null)
            {
                var pizza = await _pizzaRepository.GetById(cancellationToken, image.PizzaId.ToString());
                pizza.ModifiedOn = DateTime.UtcNow;
                pizza.ImageId = default;
                image.IsDeleted = true;
                _database.Pizzas.Update(pizza);
                _database.Images.Update(image);
                await _database.SaveChangesAsync(cancellationToken);
                return true;
            }
            throw new ImageDoesNotExistsException("the image does not exists");
        }

        public Task<bool> Exists(CancellationToken cancellationToken, string entityId, out Image existingEntity)
        {
            existingEntity = _database.Images.FirstOrDefault(i => i.Id.ToString() == entityId);
            if (existingEntity == null || !existingEntity.IsDeleted)
                throw new ImageDoesNotExistsException("");

            return Task.FromResult(true);
        }

        public async Task<List<Image>> GetAll(CancellationToken cancellationToken)
        {
            var result = _database.Images.Where(i => !i.IsDeleted).ToList();
            return await Task.FromResult(result);
        }

        public async Task<Image> GetById(CancellationToken cancellationToken, string entityId)
        {
            var image = _database.Images.Where(i => i.Id.ToString().Equals(entityId)).FirstOrDefault();
            if (image == null)
            {
                throw new ImageDoesNotExistsException("image does not exist in database");
            }
            return await Task.FromResult(image);
           
        }

        public async Task<bool> Update(CancellationToken cancellationToken, Image entity, string entityId)
        {
            var image = _database.Images.Where(i => i.Id.ToString().Equals(entityId)).FirstOrDefault();
            if (image == null)
                throw new ImageDoesNotExistsException("image does not exist"); 

            image.ModifiedOn = DateTime.Now;
            image.OriginalName = entity.OriginalName;
            image.Path = entity.Path;
            _database.Images.Update(image);
            await _database.SaveChangesAsync(cancellationToken);
            
            return true;
          
        }

        public string GetFilePath(string PizzaName)
        {
            return "C:\\Users\\user\\source\\repos\\GiorgiMatcharadze\\Pizza_Application\\RestaurantManagement\\RestaurantManagement\\Images\\" + PizzaName;
        }

        public async Task Create(CancellationToken cancellationToken, Image entity)
        {
            var pizza = await _pizzaRepository.GetById(cancellationToken, entity.PizzaId.ToString());
            pizza.ImageId = entity.Id;
            _database.Pizzas.Update(pizza);
            _database.Images.Add(entity);

            await _database.SaveChangesAsync();
        }
       
    }
}
