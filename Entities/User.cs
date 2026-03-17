using MarketAPI.Base;
using MarketAPI.RelationalEntities;
using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = null!;

        public ICollection<UserShoppingCart> UserShoppingCarts { get; set; } = new List<UserShoppingCart>();
        public ICollection<ShoppingCart> CreatedCarts { get; set; } = new List<ShoppingCart>();
        public ICollection<UserProvider> UserProviders { get; set; } = new List<UserProvider>();
    }
}
