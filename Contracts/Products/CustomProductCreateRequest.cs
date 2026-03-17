using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Contracts.Products
{
    public class CustomProductCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid? UnitOfMeasureId { get; set; }
    }
}
