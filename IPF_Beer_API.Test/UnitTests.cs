using IPF_Beer_API.Data.Model;
using Microsoft.Extensions.Configuration;

namespace IPF_Beer_API.Test
{
    public class Tests
    {
        private Data.DataContext _context;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(
                [
                    new("ConnectionStrings:WebApiDatabase", "Data Source=test.db")
                ])
                .Build();
            _context = new Data.DataContext(config);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void TestAddBeer()
        {
            if (_context.Beers.Any(x => x.Id == 1))
            {
                _context.Beers.Remove(_context.Beers.Single(x => x.Id == 1));
                _context.SaveChanges();
            }
            
            var beer = new Beer { Id = 1, Name = "John Smith", PercentageAlcoholByVolume = 4 };

            _context.Beers.Add(beer);
            _context.SaveChanges();

            var output = _context.Beers.Single(x => x.Id == 1);

            Assert.That(output.Name, Is.EqualTo(beer.Name));
            Assert.That(output.PercentageAlcoholByVolume, Is.EqualTo(beer.PercentageAlcoholByVolume));
        }

        [Test]
        public void TestAddBar()
        {
            if (_context.Bars.Any(x => x.Id == 1))
            {
                _context.Bars.Remove(_context.Bars.Single(x => x.Id == 1));
                _context.SaveChanges();
            }

            var bar = new Bar { Id = 1, Name = "Bar One", Address = "London" };

            _context.Bars.Add(bar);
            _context.SaveChanges();

            var output = _context.Bars.Single(x => x.Id == 1);

            Assert.That(output.Name, Is.EqualTo(bar.Name));
            Assert.That(output.Address, Is.EqualTo(bar.Address));
        }

        // TODO: Add more unit tests
    }
}