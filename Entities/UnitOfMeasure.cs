using MarketAPI.Base;
using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Entities
{
    public class UnitOfMeasure : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(25)]
        public string Abbreviation { get; set; } = null!;

        public ICollection<OfficialProduct> OfficialProducts { get; set; } = new List<OfficialProduct>();
        public ICollection<CustomProduct> CustomProducts { get; set; } = new List<CustomProduct>();

    }
}
