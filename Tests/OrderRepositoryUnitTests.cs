using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using Entities;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;

namespace Test
{
    public class OrderRepositoryUnitTests : IAsyncLifetime, IDisposable
    {
        private Mock<WebApiShop_329084941Context> _mockContext;
        private OrderRepository _repo;

        private Mock<WebApiShop_329084941Context> GetMockContext<T>(List<T> data, Expression<Func<WebApiShop_329084941Context, DbSet<T>>> dbSetSelector) where T : class
        {
            var mockContext = new Mock<WebApiShop_329084941Context>();
            mockContext.Setup(dbSetSelector).ReturnsDbSet(data);
            return mockContext;
        }

        // Tear-up for unit tests
        public Task InitializeAsync()
        {
            _mockContext = GetMockContext(new List<Order>(), x => x.Orders);
            _repo = new OrderRepository(_mockContext.Object);
            return Task.CompletedTask;
        }

        // Tear-down for unit tests
        public Task DisposeAsync()
        {
            _mockContext = null;
            _repo = null;
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetAllOrders_ReturnsAllOrders()
        {
            var orders = new List<Order>
            {
                new Order { OrderId = 1 },
                new Order { OrderId = 2 }
            };
            var mockContext = GetMockContext(orders, x => x.Orders);
            var repo = new OrderRepository(mockContext.Object);

            var result = await repo.getAllOrders();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetOrdersForUser_ReturnsOnlyUsersOrders()
        {
            var orders = new List<Order>
            {
                new Order { OrderId = 1, UserId = 10 },
                new Order { OrderId = 2, UserId = 10 },
                new Order { OrderId = 3, UserId = 20 }
            };
            var mockContext = GetMockContext(orders, x => x.Orders);
            var repo = new OrderRepository(mockContext.Object);
            var user = new User { Id = 10 };

            var result = await repo.getOrdersForUser(user);

            Assert.Equal(2, result.Count);
            Assert.All(result, o => Assert.Equal(10, o.UserId));
        }

        [Fact]
        public async Task GetOrderById_ExistingId_ReturnsOrder()
        {
            var orders = new List<Order> { new Order { OrderId = 5 } };
            var mockContext = GetMockContext(orders, x => x.Orders);
            var repo = new OrderRepository(mockContext.Object);

            var result = await repo.getOrderById(5);

            Assert.NotNull(result);
            Assert.Equal(5, result.OrderId);
        }

        [Fact]
        public async Task AddOrder_ValidOrder_ReturnsAddedOrder_AndSaves()
        {
            var mockContext = GetMockContext(new List<Order>(), x => x.Orders);
            var repo = new OrderRepository(mockContext.Object);
            var order = new Order { OrderSum = 15.5 };

            var result = await repo.addOrder(order);

            Assert.Equal(order, result);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateOrder_ValidUpdate_ReturnsUpdatedOrder_AndSaves()
        {
            var order = new Order { OrderId = 1, OrderSum = 10.0 };
            var mockContext = GetMockContext(new List<Order> { order }, x => x.Orders);
            var repo = new OrderRepository(mockContext.Object);

            order.OrderSum = 99.9;
            var result = await repo.updateOrder(order, order.OrderId);

            Assert.Equal(99.9, result.OrderSum);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        // synchronous fallback cleanup
        public void Dispose()
        {
            _mockContext = null;
            _repo = null;
        }
    }
}