using System;
using System.Data.Common;
using Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class DatabaseFixture : IDisposable
    {
        public ShowsCenterContext Context { get; private set; }
        private DbConnection _connection;

        public DatabaseFixture()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<ShowsCenterContext>()
                .UseSqlite(_connection)
                .Options;

            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}