namespace IPF_Beer_API.Data.Model
{
    public class Beer
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public decimal PercentageAlcoholByVolume { get; set; }
    }
}
