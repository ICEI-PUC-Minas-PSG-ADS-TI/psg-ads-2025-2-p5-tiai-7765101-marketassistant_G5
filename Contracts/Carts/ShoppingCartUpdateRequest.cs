using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Contracts.Carts
{
    public class ShoppingCartUpdateRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
