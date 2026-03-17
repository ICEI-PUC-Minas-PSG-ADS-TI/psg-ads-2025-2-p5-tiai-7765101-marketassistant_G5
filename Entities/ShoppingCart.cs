using MarketAPI.Base;
using MarketAPI.RelationalEntities;
using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Entities
{
    public class ShoppingCart : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public ICollection<UserShoppingCart> UserShoppingCarts { get; set; } = new List<UserShoppingCart>();
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
