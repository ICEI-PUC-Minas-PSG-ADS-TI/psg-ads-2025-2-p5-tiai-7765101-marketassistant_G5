namespace MarketAPI.Contracts.Products
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? UnitOfMeasureId { get; set; }
        public string? UnitOfMeasureName { get; set; }
        public string? UnitOfMeasureAbbreviation { get; set; }
        public string? ImageUrl { get; set; }
        public string? Barcode { get; set; }
    }
}
