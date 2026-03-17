using MarketAPI.Base;
using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Entities
{
    public class UserProvider : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Provider { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string ProviderUserId { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
