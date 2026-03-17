using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Contracts.Carts
{
    public class CartItemCreateRequest
    {
        [Required]
        [Range(0.0001, double.MaxValue)]
        public decimal Quantity { get; set; }

        public Guid? OfficialProductId { get; set; }
        public Guid? CustomProductId { get; set; }
    }
}
