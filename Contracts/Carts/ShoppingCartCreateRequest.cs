using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Contracts.Carts
{
    public class ShoppingCartCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
