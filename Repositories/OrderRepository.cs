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
        ShowsCenterContext _context;
        public OrderRepository(ShowsCenterContext ShowsCenterContext)
        {
            _context = ShowsCenterContext;
        }
        public async Task<List<Order>> getAllOrders()
        {
            return await _context.Orders.Include(i => i.User)
                .Include(c => c.OrderedSeats).ToListAsync();
        }
        public async Task<List<Order>> getOrdersForUser(int userId)
        {
            return await _context.Orders
                .Include(i => i.User)
                .Include(c => c.OrderedSeats)
                .Where(u => u.UserId == userId).ToListAsync();
        }
                
        public async Task<Order> getOrderById(int id)
        {
            return await _context.Orders
                .Include(i => i.User)
                .Include(c=>c.OrderedSeats)
                .FirstOrDefaultAsync(o=>o.Id == id);
        }
        public async Task<Order> addOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            if (getOrderById(order.Id) != null)
                return order;
            else
                return null;
        }

        public async Task<Order> updateOrder(Order order)
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
