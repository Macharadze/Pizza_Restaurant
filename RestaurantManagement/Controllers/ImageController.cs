using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Infrastructure.Localizations;
using RestaurantManagement.Application.Interfaces.IImage;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Api.Controllers
{
    /// <summary>
    /// Controller for managing images.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageController"/> class.
        /// </summary>
        /// <param name="imageRepository">The image repository to be injected.</param>
        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <returns>Created image</returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Image))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UploadImage(string pizzaId, string name)
        {

            try
            {
                var uploadFile = Request.Form.Files;
                foreach (var item in uploadFile)
                {
                    string filename = item.FileName;
                    string filePath = _imageRepository.GetFilePath(filename);

                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    string imagePath = filePath + $"\\{name}.png";
                    if (!System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);

                    using (FileStream strea = System.IO.File.Create(imagePath))
                    {
                        await item.CopyToAsync(strea);
                        var guidType = Guid.Parse(pizzaId);
                        var image = new Image();

                        image.Path = imagePath;
                        image.PizzaId = guidType;
                        image.OriginalName = name;

                        await _imageRepository.Create(new CancellationToken(), image);

                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, Language.NotFound);
            }
            return Ok(Language.Create);
        }

        /// <summary>
        /// Gets all images.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of images</returns>
        [HttpGet]
        //  [ProducesResponseType(200, Type = typeof(List<Image>))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var images = await _imageRepository.GetAll(cancellationToken);
            return Ok(images);
        }

        /// <summary>
        /// Gets an image by ID.
        /// </summary>
        /// <param name="imageId">Image ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Image details</returns>
        [HttpGet("{entityId}")]
        [ProducesResponseType(200, Type = typeof(Image))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(string imageId, CancellationToken cancellationToken)
        {
            try
            {
                var image = await _imageRepository.GetById(cancellationToken, imageId);

                if (image == null)
                    return NotFound();

                return Ok(image);
            }catch(Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
        }

        /// <summary>
        /// Updates an existing image.
        /// </summary>
        /// <param name="oldImageId">old Image ID</param>
        /// <param name="name">Updated image details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated image</returns>
        [HttpPut("update")]
        [ProducesResponseType(200, Type = typeof(Image))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(string oldImageId,string name, CancellationToken cancellationToken)
        {
            bool result = false;

            try
            {
                var uploadFile = Request.Form.Files;
                foreach (var item in uploadFile)
                {
                    string filename = item.FileName;
                    string filePath = _imageRepository.GetFilePath(filename);

                    
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    string imagePath = filePath + $"\\{name}.png";
                    var oldImagePath = await _imageRepository.GetById(cancellationToken, oldImageId);
                    
                    var image = new Image();

                    image.Path = imagePath;
                    image.OriginalName = name;

                    await _imageRepository.Update(new CancellationToken(), image, oldImageId);

                    if (System.IO.File.Exists(oldImagePath.Path))
                    {
                        System.IO.File.Delete(oldImagePath.Path);
                    }

                    using (FileStream strea = System.IO.File.Create(imagePath))
                    {
                        await item.CopyToAsync(strea);
                        result = true;
                        
                      
                        return Ok(Language.Update);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(404, Language.NotFound);
            }
            return Ok(result);

        }

        /// <summary>
        /// Deletes an image by path.
        /// </summary>
        /// <param name="path">Image path</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>No content</returns>
        [HttpDelete("{path}")]
        public async Task<IActionResult> Delete(string path, CancellationToken cancellationToken)
        {
            try
            {
                if (await _imageRepository.Delete(cancellationToken, path))
                {
                    System.IO.File.Delete(path);
                    return Ok(Language.Delete);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(404,Language.NotFound);
            }
        }
    }
}
