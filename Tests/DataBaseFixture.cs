using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class DatabaseFixture : IDisposable
    {
        public WebApiShop_329084941Context Context { get; private set; }

        public DatabaseFixture()
        {
            
            // Set up the test database connection and initialize the context
            var options = new DbContextOptionsBuilder<WebApiShop_329084941Context>()
                
                .UseSqlServer("Server=srv2\\pupils;Database=Tests;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;
            Context = new WebApiShop_329084941Context(options);
            Context.Database.EnsureCreated();
            SeedData();
        }

        private void SeedData()
        {
            // הוספת נתונים לדוגמה למסד הנתונים
            if (!Context.Users.Any())
            {
                Context.Users.Add(new User
                {
                    UserName = "JohnDoe",
                    FirstName = "John",
                    LastName = "Doe",
                    Password = "password123"
                });
                Context.SaveChanges();
            }
        }

        public void Dispose()
        {
            // מבצע ניקוי של החיבורים לאחר סיום הטסטים
            Context.Dispose();
        }
    }
}
