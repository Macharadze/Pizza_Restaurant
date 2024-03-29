using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Domain.Entity
{
    public record BaseId(Guid guid);
    public abstract class BaseClass
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; }
    }
}
