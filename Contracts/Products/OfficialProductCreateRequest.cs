using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Contracts.Products
{
    public class OfficialProductCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid? UnitOfMeasureId { get; set; }

        public string? ImageUrl { get; set; }
        public string? Barcode { get; set; }
    }
}
