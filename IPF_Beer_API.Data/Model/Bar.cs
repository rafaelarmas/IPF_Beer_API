namespace IPF_Beer_API.Data.Model
{
    public class Bar
    {
        public Bar()
        {
            if (BeerTypes == null)
                BeerTypes = new List<int>();
        }

        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Address { get; set; }

        public List<int> BeerTypes { get; set; }
    }
}
