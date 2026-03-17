namespace MarketAPI.Contracts.Units
{
    public class UnitOfMeasureResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
    }
}
