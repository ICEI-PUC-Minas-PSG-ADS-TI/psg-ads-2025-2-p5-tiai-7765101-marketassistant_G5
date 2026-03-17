using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Contracts.Carts
{
    public class CartItemUpdateRequest
    {
        [Required]
        [Range(0.0001, double.MaxValue)]
        public decimal Quantity { get; set; }
    }
}
