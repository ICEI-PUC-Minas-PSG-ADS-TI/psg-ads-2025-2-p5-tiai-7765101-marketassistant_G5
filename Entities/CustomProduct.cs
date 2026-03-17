using MarketAPI.Base;
using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Entities
{
    public class CustomProduct : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public Guid? UnitOfMeasureId { get; set; }
        public UnitOfMeasure? UnitOfMeasure { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

}
