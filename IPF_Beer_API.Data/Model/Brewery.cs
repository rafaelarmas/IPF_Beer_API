namespace IPF_Beer_API.Data.Model
{
    public class Brewery
    {
        public Brewery()
        {
            if (BeerTypes == null)
                BeerTypes = new List<int>();
        }

        public int Id { get; set; }

        public required string Name { get; set; }

        public List<int> BeerTypes { get; set; }
    }
}
