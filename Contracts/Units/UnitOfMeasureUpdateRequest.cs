using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Contracts.Units
{
    public class UnitOfMeasureUpdateRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(25)]
        public string Abbreviation { get; set; } = string.Empty;
    }
}
