using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        WebApiShop_329084941Context _context;
        public OrderRepository(WebApiShop_329084941Context webApiShop_329084941Context)
        {
            _context = webApiShop_329084941Context;
        }
        public async Task<List<Order>> getAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task<List<Order>> getOrdersForUser(User user)
        {
            return await _context.Orders.Where(u => u.UserId == user.Id).ToListAsync();
        }
        public async Task<Order> getOrderById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
        public async Task<Order> addOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            if (getOrderById(order.OrderId) != null)
                return order;
            else
                return null;
        }

        public async Task<Order> updateOrder(Order order, int id)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        //public async Task deleteOrder(int id)
        //{
        //    await _context.Orders.ExecuteDeleteAsync(await .getOrderById(id));
        //    await _context.SaveChangesAsync();
        //}
    }
}
