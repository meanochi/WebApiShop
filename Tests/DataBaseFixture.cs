using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Tests
{
    public class DatabaseFixture : IDisposable
    {
        public WebApiShop_329084941Context Context { get; private set; }

        public DatabaseFixture()
        {

            // Set up the test database connection and initialize the context
            var options = new DbContextOptionsBuilder<WebApiShop_329084941Context>()
                .UseSqlServer("Data Source=srv2\\pupils;Initial Catalog=WebApiShop_329084941;Integrated Security=True;Pooling=False;Trust Server Certificate=True;")
                .Options;
            Context = new WebApiShop_329084941Context(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            // Clean up the test database after all tests are completed
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
